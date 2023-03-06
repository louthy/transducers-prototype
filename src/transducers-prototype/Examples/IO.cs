using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record IO<Env, X, A>(SumTransducer<Env, X, Env, A> MorphismValue) : 
    KArr<IO<Env, X>, Env, X, Env, A>
{
    Transducer<Sum<Env, Env>, Sum<X, A>> KArr<IO<Env, X>, Sum<Env, Env>, Sum<X, A>>.Morphism => 
        MorphismValue;

    SumTransducer<Env, X, Env, A> KArr<IO<Env, X>, Env, X, Env, A>.Morphism => 
        MorphismValue;
}

public readonly struct IO<Env, X> : MonadSum<IO<Env, X>, Env, X>
{
    public static KArr<IO<Env, X>, Env, X, Env, B> BiMap<A, B>(
        KArr<IO<Env, X>, Env, X, Env, A> fab, 
        Transducer<X, X> Left, 
        Transducer<A, B> Right)
    {
        throw new NotImplementedException();
    }

    public static KArr<IO<Env, X>, Env, X, Env, A> Lift<A>(SumTransducer<Env, X, Env, A> f)
    {
        throw new NotImplementedException();
    }

    public static KArr<IO<Env, X>, Env, X, Env, B> Bind<A, B>(
        KArr<IO<Env, X>, Env, X, Env, A> mx, 
        Transducer<A, KArr<IO<Env, X>, Env, X, Env, B>> f)
    {
        throw new NotImplementedException();
    }
}