namespace LanguageExt.HKT;

public interface MonadTrans<T, M>
    where M : Monad<M>
    where T : MonadTrans<T, M>
{
    KArr<T, KArr<M, A, B>, KArr<M, A, B>> Lift<A, B, C, D>(KArr<M, A, B> mab);
}

