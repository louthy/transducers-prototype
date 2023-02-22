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
    public static abstract K<M, X, Z, A, C> Bind<X, Y, Z, A, B, C>(
        K<M, Sum<X, A>, Sum<Y, B>> mx, 
        Transducer<B, K<M, X, Z, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, X, Z, A, C> Bind<X, Y, Z, A, B, C>(
        K<M, X, Y, A, B> mx, 
        Func<B, K<M, X, Z, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, X, Y, A, B> Flatten<X, Y, A, B>(
        K<M, X, Y, A, K<M, X, Y, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, X, Y, A, B>>());
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
    public static abstract K<M, X, X, A, C> Bind<A, B, C>(
        K<M, X, X, A, B> mx, 
        Transducer<B, K<M, X, X, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, X, X, A, C> Bind<A, B, C>(
        K<M, X, X, A, B> mx, 
        Func<B, K<M, X, X, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, Sum<X, A>, Sum<X, B>> Flatten<A, B>(
        K<M, X, X, A, K<M, X, X, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, X, X, A, B>>());
}

/// <summary>
/// Monad bind trait with fixed left type and input type
/// </summary>
public interface MonadSum<M, X, A> : ApplicativeSum<M, X, A> 
    where M : MonadSum<M, X, A>
{
    /// <summary>
    /// Monad bind
    /// </summary>
    public static abstract K<M, X, X, A, C> Bind<B, C>(
        K<M, X, X, A, B> mx, 
        Transducer<B, K<M, X, X, A, C>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, X, X, A, C> Bind<B, C>(
        K<M, X, X, A, B> mx, 
        Func<B, K<M, X, X, A, C>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual K<M, X, X, A, B> Flatten<B>(
        K<M, X, X, A, K<M, X, X, A, B>> mmx) =>
        M.Bind(mmx, Transducer.identity<K<M, X, X, A, B>>());
}
