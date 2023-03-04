using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Identity<Env, X, A>(SumTransducer<Env, X, Env, A> MorphismValue) : 
    KArr<Identity<Env, X>, Env, X, Env, A>
{
    Transducer<Sum<Env, Env>, Sum<X, A>> KArr<Identity<Env, X>, Sum<Env, Env>, Sum<X, A>>.Morphism => 
        MorphismValue;

    SumTransducer<Env, X, Env, A> KArr<Identity<Env, X>, Env, X, Env, A>.Morphism => 
        MorphismValue;
}

public readonly struct Identity<Env, X> : MonadSum2<Identity<Env, X>, Env, X>
{
    public static KArr<Identity<Env, X>, Env, X, Env, B> BiMap<A, B>(
        KArr<Identity<Env, X>, Env, X, Env, A> fab, 
        Transducer<X, X> Left, 
        Transducer<A, B> Right)
    {
        throw new NotImplementedException();
    }

    public static KArr<Identity<Env, X>, Env, X, Env, A> Lift<A>(SumTransducer<Env, X, Env, A> f)
    {
        throw new NotImplementedException();
    }

    public static KArr<Identity<Env, X>, Env, X, Env, B> Bind<A, B>(
        KArr<Identity<Env, X>, Env, X, Env, A> mx, 
        Transducer<A, KArr<Identity<Env, X>, Env, X, Env, B>> f)
    {
        throw new NotImplementedException();
    }
}