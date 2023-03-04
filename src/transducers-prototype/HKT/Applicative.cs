namespace LanguageExt.HKT;

/// <summary>
/// Applicative pure trait
/// </summary>
public interface Applicative<F> : Functor<F> 
    where F : Applicative<F>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract KArr<F, A, B> Lift<A, B>(Transducer<A, B> f);
    
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, A, B> Lift<A, B>(Func<A, B> f) =>
        F.Lift(Transducer.lift(f));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, A, B> Pure<A, B>(B value) =>
        F.Lift(Transducer.constant<A, B>(value));
}

/// <summary>
/// Applicative pure trait with fixed input type
/// </summary>
public interface Applicative<F, A> : Functor<F, A> 
    where F : Applicative<F, A>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract KArr<F, A, B> Lift<B>(Transducer<A, B> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, A, B> Lift<B>(Func<A, B> f) =>
        F.Lift(Transducer.lift(f));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, A, B> Pure<B>(B value) =>
        F.Lift(Transducer.constant<A, B>(value));
}
