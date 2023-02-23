namespace LanguageExt.HKT;

public interface MonadTrans<T, M>
    where M : Monad<M>
    where T : MonadTrans<T, M>
{
    K<T, K<M, A, B>, K<M, A, B>> Lift<A, B, C, D>(K<M, A, B> mab);
}

