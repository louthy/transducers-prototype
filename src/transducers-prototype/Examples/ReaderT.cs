#nullable enable
using LanguageExt;
using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record ReaderT<M, Env>
    where M : Monad<M, Env> 
{
    public static ReaderT<M, Env, A> Pure<A>(A value) => 
        new (M.Pure(Transducer.constant<Env, A>(value)).Morphism);
    
    public static readonly ReaderT<M, Env, Env> Ask =
        new (M.Pure(Transducer.lift<Env, Env>(env => env)).Morphism);
}

public record ReaderT<M, Env, A>(Transducer<Env, Transducer<Env, A>> MorphismValue) :
    KArr<M, Env, Transducer<Env, A>>
    where M : Monad<M, Env>
{
    public Transducer<Env, Transducer<Env, A>> Morphism =>
        MorphismValue;
    
    public ReaderT<M, Env, B> Map<B>(Func<A, B> f) =>
        new(Morphism.Map(mx => mx.Map(f)));

    public ReaderT<M, Env, B> Select<B>(Func<A, B> f) =>
        new(Morphism.Map(mx => mx.Map(f)));

    public ReaderT<M, Env, B> Bind<B>(Func<A, ReaderT<M, Env, B>> f) =>
        new(Morphism.Map(mx => mx.Map(x => f(x).Morphism)).FlattenT());

    public ReaderT<M, Env, C> SelectMany<B, C>(Func<A, ReaderT<M, Env, B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
    
    public A Run(Env env) =>
        Morphism.Invoke(env, default, new OuterReduce(env)) switch
        {
            TComplete<A?> r => r.Value ?? throw new InvalidOperationException(),
            TCancelled<A?> => throw new OperationCanceledException(),
            TFail<A?> fail => fail.Error.Throw<A>(),
            _ => throw new InvalidOperationException()
        };
    
    record OuterReduce(Env Env) : Reducer<A?, Transducer<Env, A>>
    {
        public override TResult<A?> Run(TState st, A? s, Transducer<Env, A> inner) =>
            inner.Transform(InnerReduce.Default).Run(st, s, Env);
    }    
    
    record InnerReduce : Reducer<A?, A>
    {
        public static readonly InnerReduce Default = new();
        
        public override TResult<A?> Run(TState st, A? s, A x) =>
            TResult.Complete((A?)x);
    }    
}

public struct MReaderT<M, Env> : Monad<MReaderT<M, Env>, Env>
    where M : Monad<M, Env>
{
    public static KArr<MReaderT<M, Env>, Env, C> Map<B, C>(KArr<MReaderT<M, Env>, Env, B> fab, Transducer<B, C> f) =>
        new Wrap<Env, C>(Transducer.compose(fab.Morphism, f));

    public static KArr<MReaderT<M, Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new Wrap<Env, B>(f);

    public static KArr<MReaderT<M, Env>, Env, B> Bind<A, B>(
        KArr<MReaderT<M, Env>, Env, A> mx, 
        Transducer<A, KArr<MReaderT<M, Env>, Env, B>> f) =>
        new Wrap<Env, B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());

    record Wrap<A, B>(Transducer<A, B> F) : KArr<MReaderT<M, Env>, A, B>
    {
        public Transducer<A, B> Morphism => F;
    }
}
