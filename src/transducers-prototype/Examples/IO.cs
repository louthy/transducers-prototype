using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record IO<Env, A>(Transducer<Env, A> MorphismValue) : 
    KArr<IO<Env>, Env, A>
{
    Transducer<Env, A> KArr<IO<Env>, Env, A>.Morphism => 
        MorphismValue;
}

public readonly struct IO<Env> : Monad<IO<Env>, Env>
{
    public static KArr<IO<Env>, Env, C> Map<B, C>(KArr<IO<Env>, Env, B> fab, Transducer<B, C> f) =>
        new IO<Env, C>(Transducer.compose(fab.Morphism, f));

    public static KArr<IO<Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new IO<Env, B>(f);

    public static KArr<IO<Env>, Env, B> Bind<A, B>(KArr<IO<Env>, Env, A> mx, Transducer<A, KArr<IO<Env>, Env, B>> f) =>
        new IO<Env, B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());

}