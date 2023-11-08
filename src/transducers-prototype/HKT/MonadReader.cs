namespace LanguageExt.HKT;

/// <summary>
/// Monad bind trait
/// </summary>
public interface Monad<M> : Applicative<M> 
    where M : Monad<M>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, Unit, B> Bind<A, B>(KArr<M, Unit, A> mx, Transducer<A, KArr<M, Unit, B>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Unit, B> Bind<A, B>(KArr<M, Unit, A> mx, Func<A, KArr<M, Unit, B>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Unit, A> Flatten<A>(KArr<M, Unit, KArr<M, Unit, A>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, Unit, A>>());
}

/// <summary>
/// Monad bind trait with fixed input type
/// </summary>
public interface MonadReader<M, Env> : Applicative<M, Env> 
    where M : MonadReader<M, Env>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, Env, B> Bind<A, B>(KArr<M, Env, A> mx, Transducer<A, KArr<M, Env, B>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, B> Bind<A, B>(KArr<M, Env, A> mx, Func<A, KArr<M, Env, B>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, A> Flatten<A>(KArr<M, Env, KArr<M, Env, A>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, Env, A>>());
}
