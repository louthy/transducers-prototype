#nullable enable
using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// Either transformer
/// </summary>
public record EitherT<M, L, R>(Transducer<Unit, Transducer<Unit, Sum<L, R>>> MorphismValue) : 
    KArr<M, Unit, Transducer<Unit, Sum<L, R>>>
    where M : Monad<M, Unit>
{
    public Transducer<Unit, Transducer<Unit, Sum<L, R>>> Morphism => 
        MorphismValue;

    public static EitherT<M, Unit, L, R> Right(R value) =>
        new (M.Pure(Transducer.Pure(Sum<L, R>.Right(value))).Morphism);

    public static EitherT<M, Unit, L, R> Left(L value) =>
        new (M.Pure(Transducer.Pure(Sum<L, R>.Left(value))).Morphism);
    
    public EitherT<M, Unit, L, B> MapRight<B>(Func<R, B> f) =>
        new(Morphism.Map(mx => mx.MapRight(f)));

    public EitherT<M, Unit, B, R> MapLeft<B>(Func<L, B> f) =>
        new(Morphism.Map(mx => mx.MapLeft(f)));

    public EitherT<M, Unit, A, B> BiMap<A, B>(Func<L, A> Left, Func<R, B> Right) =>
        new(Morphism.Map(mx => mx.BiMap(Left, Right)));

    public EitherT<M, Unit, L, B> Select<B>(Func<R, B> f) =>
        new(Morphism.Map(mx => mx.MapRight(f)));

    public EitherT<M, Unit, L, B> Bind<B>(Func<R, EitherT<M, Unit, L, B>> f) =>
        new(Morphism.Map(mx => mx.MapRight(x => f(x).Morphism)).FlattenT());

    public EitherT<M, Unit, L, C> SelectMany<B, C>(Func<R, EitherT<M, Unit, L, B>> bind, Func<R, B, C> project) =>
        Bind(x => bind(x).MapRight(y => project(x, y)));

    public A Match<A>(Unit env, Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom = null) =>
        Morphism.Invoke(env, default, OuterReduce.Default) switch
        {
            TComplete<Sum<L, R>> inner =>
                inner switch
                {
                    SumRight<L, R> right => Right(right.Value),
                    SumLeft<L, R> left => Left(left.Value),
                    _ => Bottom is null ? throw new NotSupportedException() : Bottom()
                },

            TCancelled<Sum<L, R>> inner =>
                throw new OperationCanceledException(),

            TFail<Sum<L, R>> inner =>
                inner.Error.Throw<A>(),

            _ => Bottom is null ? throw new NotSupportedException() : Bottom()
        };
    
    record OuterReduce : Reducer<Sum<L, R>?, Transducer<Unit, Sum<L, R>>>
    {
        public static readonly OuterReduce Default = new();
        
        public override TResult<Sum<L, R>?> Run(TState st, Sum<L, R>? s, Transducer<Unit, Sum<L, R>> inner) =>
            inner.Transform(InnerReduce.Default).Run(st, s, default);
    }    
    
    record InnerReduce : Reducer<Sum<L, R>?, Sum<L, R>>
    {
        public static readonly InnerReduce Default = new();
        
        public override TResult<Sum<L, R>?> Run(TState st, Sum<L, R>? s, Sum<L, R> x) =>
            TResult.Complete((Sum<L, R>?)x);
    }   
}

/// <summary>
/// Either transformer
/// </summary>
public record EitherT<M, Env, L, R>(Transducer<Env, Transducer<Env, Sum<L, R>>> MorphismValue) : 
    KArr<M, Env, Transducer<Env, Sum<L, R>>>
    where M : Monad<M, Env>
{
    public Transducer<Env, Transducer<Env, Sum<L, R>>> Morphism => 
        MorphismValue;

    public static EitherT<M, Env, L, R> Right(R value) =>
        new (M.Pure(Transducer.constant<Env, Sum<L, R>>(Sum<L, R>.Right(value))).Morphism);

    public static EitherT<M, Env, L, R> Left(L value) =>
        new (M.Pure(Transducer.constant<Env, Sum<L, R>>(Sum<L, R>.Left(value))).Morphism);

    public static EitherT<M, Env, L, R> Lift(Transducer<Env, Transducer<Env, R>> ma) =>
        new(ma.Morphism.Map(m => m.Map(r => Right(r).Morphism)).FlattenT());
    
    public EitherT<M, Env, L, B> MapRight<B>(Func<R, B> f) =>
        new(Morphism.Map(mx => mx.MapRight(f)));

    public EitherT<M, Env, B, R> MapLeft<B>(Func<L, B> f) =>
        new(Morphism.Map(mx => mx.MapLeft(f)));

    public EitherT<M, Env, A, B> BiMap<A, B>(Func<L, A> Left, Func<R, B> Right) =>
        new(Morphism.Map(mx => mx.BiMap(Left, Right)));

    public EitherT<M, Env, L, B> Select<B>(Func<R, B> f) =>
        new(Morphism.Map(mx => mx.MapRight(f)));

    public EitherT<M, Env, L, B> Bind<B>(Func<R, EitherT<M, Env, L, B>> f) =>
        new(Morphism.Map(mx => mx.MapRight(x => f(x).Morphism)).FlattenT());

    public EitherT<M, Env, L, C> SelectMany<B, C>(Func<R, EitherT<M, Env, L, B>> bind, Func<R, B, C> project) =>
        Bind(x => bind(x).MapRight(y => project(x, y)));

    public A Match<A>(Env env, Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom = null) =>
        Morphism.Invoke(env, default, new OuterReduce(env)) switch
        {
            TComplete<Sum<L, R>> inner =>
                inner switch
                {
                    SumRight<L, R> right => Right(right.Value),
                    SumLeft<L, R> left => Left(left.Value),
                    _ => Bottom is null ? throw new NotSupportedException() : Bottom()
                },

            TCancelled<Sum<L, R>> inner =>
                throw new OperationCanceledException(),

            TFail<Sum<L, R>> inner =>
                inner.Error.Throw<A>(),

            _ => Bottom is null ? throw new NotSupportedException() : Bottom()
        };
    
    record OuterReduce(Env Env) : Reducer<Sum<L, R>?, Transducer<Env, Sum<L, R>>>
    {
        public override TResult<Sum<L, R>?> Run(TState st, Sum<L, R>? s, Transducer<Env, Sum<L, R>> inner) =>
            inner.Transform(InnerReduce.Default).Run(st, s, Env);
    }    
    
    record InnerReduce : Reducer<Sum<L, R>?, Sum<L, R>>
    {
        public static readonly InnerReduce Default = new();
        
        public override TResult<Sum<L, R>?> Run(TState st, Sum<L, R>? s, Sum<L, R> x) =>
            TResult.Complete((Sum<L, R>?)x);
    }   
}