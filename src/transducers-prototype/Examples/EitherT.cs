#nullable enable
using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// Either transformer
/// </summary>
public readonly struct EitherT<M, L, R> : KArr<M, Unit, Sum<L, R>>
    where M : Monad<M>
{
    readonly Transducer<Unit, Sum<L, R>> morphism;
    
    EitherT(Transducer<Unit, Sum<L, R>> morphism) =>
        this.morphism = morphism;
    
    public Transducer<Unit, Sum<L, R>> Morphism => 
        morphism;

    public static EitherT<M, L, R> Right(R value) =>
        new (M.Pure(Transducer.Pure(Sum<L, R>.Right(value))).Morphism.Flatten());

    public static EitherT<M, L, R> Left(L value) =>
        new (M.Pure(Transducer.Pure(Sum<L, R>.Left(value))).Morphism.Flatten());
    
    public static EitherT<M, L, R> Lift(Transducer<Unit, R> ma) =>
        Lift(ma.Map(Sum<L, R>.Right));
    
    public static EitherT<M, L, R> Lift(Transducer<Unit, Sum<L, R>> ma) =>
        new (M.Pure(ma).Morphism.Flatten());
    
    public EitherT<M, L, B> MapRight<B>(Func<R, B> f) =>
        new(Morphism.MapRight(f));

    public EitherT<M, B, R> MapLeft<B>(Func<L, B> f) =>
        new(Morphism.MapLeft(f));

    public EitherT<M, A, B> BiMap<A, B>(Func<L, A> Left, Func<R, B> Right) =>
        new(Morphism.BiMap(Left, Right));

    public EitherT<M, L, B> Select<B>(Func<R, B> f) =>
        new(Morphism.MapRight(f));

    public EitherT<M, L, B> Bind<B>(Func<R, EitherT<M, L, B>> f) =>
        new(Morphism.MapRight(x => f(x).Morphism).Flatten());

    public EitherT<M, L, C> SelectMany<B, C>(Func<R, EitherT<M, L, B>> bind, Func<R, B, C> project) =>
        Bind(x => bind(x).MapRight(y => project(x, y)));

    public A Match<A>(Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom = null) =>
        Morphism.Invoke1(default) switch
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
}

/// <summary>
/// Either transformer
/// </summary>
public readonly struct EitherT<M, Env, L, R>: 
    KArr<M, Env, Sum<L, R>>
    where M : MonadReader<M, Env>
{
    readonly Transducer<Env, Sum<L, R>> morphism;
    
    EitherT(Transducer<Env, Sum<L, R>> morphism) =>
        this.morphism = morphism;
    
    public Transducer<Env, Sum<L, R>> Morphism => 
        morphism;

    public static EitherT<M, Env, L, R> Right(R value) =>
        new (M.Pure(Transducer.constant<Env, Sum<L, R>>(Sum<L, R>.Right(value))).Morphism.Flatten());

    public static EitherT<M, Env, L, R> Left(L value) =>
        new (M.Pure(Transducer.constant<Env, Sum<L, R>>(Sum<L, R>.Left(value))).Morphism.Flatten());

    public static EitherT<M, Env, L, R> Lift(Transducer<Env, R> ma) =>
        Lift(ma.Map(Sum<L, R>.Right));

    public static EitherT<M, Env, L, R> Lift(Transducer<Env, Sum<L, R>> ma) =>
        new (M.Pure(ma).Morphism.Flatten());
    
    public EitherT<M, Env, L, B> MapRight<B>(Func<R, B> f) =>
        new(Morphism.MapRight(f));

    public EitherT<M, Env, B, R> MapLeft<B>(Func<L, B> f) =>
        new(Morphism.MapLeft(f));

    public EitherT<M, Env, A, B> BiMap<A, B>(Func<L, A> Left, Func<R, B> Right) =>
        new(Morphism.BiMap(Left, Right));

    public EitherT<M, Env, L, B> Select<B>(Func<R, B> f) =>
        new(Morphism.MapRight(f));

    public EitherT<M, Env, L, B> Bind<B>(Func<R, EitherT<M, Env, L, B>> f) =>
        new(Morphism.MapRight(x => f(x).Morphism).Flatten());

    public EitherT<M, Env, L, C> SelectMany<B, C>(Func<R, EitherT<M, Env, L, B>> bind, Func<R, B, C> project) =>
        Bind(x => bind(x).MapRight(y => project(x, y)));

    public A Match<A>(Env env, Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom = null) =>
        Morphism.Invoke1(env) switch
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
}