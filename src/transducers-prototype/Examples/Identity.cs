using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Identity<Env, A>(Transducer<Env, A> MorphismValue) : 
    KArr<Identity<Env>, Env, A>
{
    public Transducer<Env, A> Morphism => 
        MorphismValue;
}

public readonly struct Identity<Env> : Monad<Identity<Env>, Env>
{
    public static KArr<Identity<Env>, Env, C> Map<B, C>(KArr<Identity<Env>, Env, B> fab, Transducer<B, C> f)
    {
        throw new NotImplementedException();
    }

    public static KArr<Identity<Env>, Env, B> Lift<B>(Transducer<Env, B> f)
    {
        throw new NotImplementedException();
    }

    public static KArr<Identity<Env>, Env, B> Bind<A, B>(KArr<Identity<Env>, Env, A> mx, Transducer<A, KArr<Identity<Env>, Env, B>> f)
    {
        throw new NotImplementedException();
    }
}