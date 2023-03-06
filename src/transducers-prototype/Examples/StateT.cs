using LanguageExt.HKT;

namespace LanguageExt.Examples;

/// <summary>
/// State transformer
/// </summary>
public record StateT<M, S, X, A>(Transducer<S, SumTransducer<S, X, S, (S State, A Value)>> MorphismValue) : 
    KArr<M, S, SumTransducer<S, X, S, A>>
    where M : Monad<M, S>
{
    public Transducer<S, SumTransducer<S, X, S, A>> Morphism { get; }
}

/*
public readonly struct StateT<M, Env, L> : MonadSum2<M, Env, L>
    where M : MonadSum2<M, Env, L>
{
}
#1#
*/
