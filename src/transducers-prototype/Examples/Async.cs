using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Async<Env, A>(Transducer<Env, ValueTask<A>> MorphismValue) : 
    KArr<Async<Env>, Env, A>
{
    public Transducer<Env, A> Morphism =>
        new AsyncTransducer<Env, A>(MorphismValue);
}

public readonly struct Async<Env> : Monad<Async<Env>, Env>
{
    public static KArr<Async<Env>, Env, C> Map<B, C>(KArr<Async<Env>, Env, B> fab, Transducer<B, C> f) =>
        new Async<Env, C>(Transducer.compose(fab.Morphism, f.Map(static x => new ValueTask<C>(x))));

    public static KArr<Async<Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new Async<Env, B>(f.Map(static x => new ValueTask<B>(x)));

    public static KArr<Async<Env>, Env, B> Bind<A, B>(
        KArr<Async<Env>, Env, A> mx, 
        Transducer<A, KArr<Async<Env>, Env, B>> f) =>
        new Async<Env, B>(
            Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism))
                .Flatten()
                .Map(static x => new ValueTask<B>(x)));
}