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
    public static abstract KArr<F, Unit, A> Lift<A>(Transducer<Unit, A> f);
    
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, Unit, A> Lift<A>(Func<Unit, A> f) =>
        F.Lift(Transducer.lift(f));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, Unit, B> Pure<B>(B value) =>
        F.Lift(Transducer.constant<Unit, B>(value));
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
