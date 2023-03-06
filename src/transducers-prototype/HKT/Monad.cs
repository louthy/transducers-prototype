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
    public static abstract KArr<M, Env, B> Bind<Env, A, B>(KArr<M, Env, A> mx, Transducer<A, KArr<M, Env, B>> f);

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, B> Bind<Env, A, B>(KArr<M, Env, A> mx, Func<A, KArr<M, Env, B>> f) =>
        M.Bind(mx, Transducer.lift(f));

    /// <summary>
    /// Monad bind
    /// </summary>
    public static virtual KArr<M, Env, A> Flatten<Env, A>(KArr<M, Env, KArr<M, Env, A>> mmx) =>
        M.Bind(mmx, Transducer.identity<KArr<M, Env, A>>());
}

/// <summary>
/// Monad bind trait with fixed input type
/// </summary>
public interface Monad<M, Env> : Applicative<M, Env> 
    where M : Monad<M, Env>
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
