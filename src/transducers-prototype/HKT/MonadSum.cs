namespace LanguageExt.HKT;

/// <summary>
/// Monad bind trait
/// </summary>
public interface MonadSum<M> : ApplicativeSum<M> 
    where M : MonadSum<M>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, X, Z, A, C> Bind<X, Y, Z, A, B, C>(
        KArr<M, Sum<X, A>, Sum<Y, B>> mx, 
        Transducer<B, KArr<M, X, Z, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, X, Z, A, C> Bind<X, Y, Z, A, B, C>(
        KArr<M, X, Y, A, B> mx, 
        Func<B, KArr<M, X, Z, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, X, Y, A, B> Flatten<X, Y, A, B>(
        KArr<M, X, Y, A, KArr<M, X, Y, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, X, Y, A, B>>());
}

/// <summary>
/// Monad bind trait with fixed left type
/// </summary>
public interface MonadSum<M, X> : ApplicativeSum<M, X> 
    where M : MonadSum<M, X>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, Env, X, Env, B> Bind<Env, A, B>(
        KArr<M, Env, X, Env, A> mx, 
        Transducer<A, KArr<M, Env, X, Env, B>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, X, Env, B> Bind<Env, A, B>(
        KArr<M, Env, X, Env, A> mx, 
        Func<A, KArr<M, Env, X, Env, B>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, X, Env, A> Flatten<Env, A>(
        KArr<M, Env, X, Env, KArr<M, Env, X, Env, A>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, Env, X, Env, A>>());
}

/// <summary>
/// Monad bind trait with fixed left type and input type
/// </summary>
public interface MonadSum2<M, Env, X> : ApplicativeSum2<M, Env, X> 
    where M : MonadSum2<M, Env, X>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract KArr<M, Env, X, Env, B> Bind<A, B>(
        KArr<M, Env, X, Env, A> mx, 
        Transducer<A, KArr<M, Env, X, Env, B>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, X, Env, B> Bind<A, B>(
        KArr<M, Env, X, Env, A> mx, 
        Func<A, KArr<M, Env, X, Env, B>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, X, Env, A> Flatten<A>(
        KArr<M, Env, X, Env, KArr<M, Env, X, Env, A>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, Env, X, Env, A>>());
}
