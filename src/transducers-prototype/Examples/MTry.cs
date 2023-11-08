using LanguageExt.HKT;

namespace LanguageExt.Examples;

public readonly struct Try<Env, A> : 
    KArr<MTry<Env>, Env, A>
{
    readonly Transducer<Env, A> morphism;

    public Try(Transducer<Env, A> morphism) =>
        this.morphism = morphism;
    
    Transducer<Env, A> KArr<MTry<Env>, Env, A>.Morphism => 
        morphism;
}

public readonly struct MTry<Env> : MonadReader<MTry<Env>, Env>
{
    public static KArr<MTry<Env>, Env, C> Map<B, C>(KArr<MTry<Env>, Env, B> fab, Transducer<B, C> f) =>
        new Try<Env, C>(Transducer.compose(fab.Morphism, f));

    public static KArr<MTry<Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new Try<Env, B>(f);

    public static KArr<MTry<Env>, Env, B> Bind<A, B>(KArr<MTry<Env>, Env, A> mx, Transducer<A, KArr<MTry<Env>, Env, B>> f) =>
        new Try<Env, B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());
}
