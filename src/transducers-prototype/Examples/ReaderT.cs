#nullable enable
using System.Diagnostics;
using LanguageExt;
using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record ReaderT<M, Env>
    where M : Monad<M, Env>
{
    public static ReaderT<M, Env, A> Pure<A>(A value) =>
        ReaderT<M, Env, A>.Pure(value);
    
    public static readonly ReaderT<M, Env, Env> Ask =
        ReaderT<M, Env, Env>.Lift(Transducer.lift<Env, Env>(e => e));
}

public readonly struct ReaderT<M, Env, A>:
    KArr<M, Env, A>
    where M : Monad<M, Env>
{
    readonly Transducer<Env, A> transformer;

    ReaderT(Transducer<Env, A> transformer) =>
        this.transformer = transformer;

    public Transducer<Env, A> Morphism =>
        transformer;
    
    public static ReaderT<M, Env, A> Pure(A value) => 
        new (M.Pure(Transducer.constant<Env, A>(value)).Morphism.Flatten());
    
    public static ReaderT<M, Env, A> Lift(Transducer<Env, A> ma) => 
        new (M.Pure(ma).Morphism.Flatten());
    
    public ReaderT<M, Env, B> Map<B>(Func<A, B> f) =>
        new(Morphism.Map(f));

    public ReaderT<M, Env, B> Select<B>(Func<A, B> f) =>
        new(Morphism.Map(f));

    public ReaderT<M, Env, B> Bind<B>(Func<A, ReaderT<M, Env, B>> f) =>
        new(Morphism.Map(x => f(x).Morphism).Flatten());

    public ReaderT<M, Env, C> SelectMany<B, C>(Func<A, ReaderT<M, Env, B>> bind, Func<A, B, C> project) =>
        Bind(x => bind(x).Map(y => project(x, y)));
    
    public A Run(Env env) =>
        Morphism.Invoke1(env) switch
        {
            TComplete<A?> r => r.Value ?? throw new InvalidOperationException(),
            TCancelled<A?> => throw new OperationCanceledException(),
            TFail<A?> fail => fail.Error.Throw<A>(),
            _ => throw new UnreachableException()
        };
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
