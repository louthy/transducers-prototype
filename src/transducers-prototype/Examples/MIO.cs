using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record IO<Env, A>(Transducer<Env, A> MorphismValue) : 
    KArr<MIO<Env>, Env, A>
{
    Transducer<Env, A> KArr<MIO<Env>, Env, A>.Morphism => 
        MorphismValue;
}

public readonly struct MIO<Env> : MonadReader<MIO<Env>, Env>
{
    public static KArr<MIO<Env>, Env, C> Map<B, C>(KArr<MIO<Env>, Env, B> fab, Transducer<B, C> f) =>
        new IO<Env, C>(Transducer.compose(fab.Morphism, f));

    public static KArr<MIO<Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new IO<Env, B>(f);

    public static KArr<MIO<Env>, Env, B> Bind<A, B>(KArr<MIO<Env>, Env, A> mx, Transducer<A, KArr<MIO<Env>, Env, B>> f) =>
        new IO<Env, B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());
}
