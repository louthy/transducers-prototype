using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Identity<A>(Transducer<Unit, A> MorphismValue) : 
    KArr<MIdentity, Unit, A>
{
    public Transducer<Unit, A> Morphism => 
        MorphismValue;
}

public record Identity<Env, A>(Transducer<Env, A> MorphismValue) : 
    KArr<MIdentity<Env>, Env, A>
{
    public Transducer<Env, A> Morphism => 
        MorphismValue;
}

public readonly struct MIdentity : Monad<MIdentity>
{
    public static KArr<MIdentity, Unit, C> Map<B, C>(KArr<MIdentity, Unit, B> fab, Transducer<B, C> f) =>
        new Identity<C>(Transducer.compose(fab.Morphism, f));

    public static KArr<MIdentity, Unit, B> Lift<B>(Transducer<Unit, B> f) =>
        new Identity<B>(f);

    public static KArr<MIdentity, Unit, B> Bind<A, B>(KArr<MIdentity, Unit, A> mx, Transducer<A, KArr<MIdentity, Unit, B>> f) =>
        new Identity<B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());
}

public readonly struct MIdentity<Env> : MonadReader<MIdentity<Env>, Env>
{
    public static KArr<MIdentity<Env>, Env, C> Map<B, C>(KArr<MIdentity<Env>, Env, B> fab, Transducer<B, C> f) =>
        new Identity<Env, C>(Transducer.compose(fab.Morphism, f));

    public static KArr<MIdentity<Env>, Env, B> Lift<B>(Transducer<Env, B> f) =>
        new Identity<Env, B>(f);

    public static KArr<MIdentity<Env>, Env, B> Bind<A, B>(KArr<MIdentity<Env>, Env, A> mx, Transducer<A, KArr<MIdentity<Env>, Env, B>> f) =>
        new Identity<Env, B>(Transducer.compose(mx.Morphism, f.Map(static x => x.Morphism)).Flatten());
}