namespace LanguageExt.HKT;

/// <summary>
/// Applicative pure trait
/// </summary>
public interface ApplicativeSum<F> : FunctorSum<F>
    where F : ApplicativeSum<F>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract K<F, X, Y, A, B> Lift<X, Y, A, B>(SumTransducer<X, Y, A, B> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, Y, A, B> Lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> f) =>
        F.Lift(SumTransducer.lift(f));

    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, X, Y, A, B> Lift<X, Y, A, B>(Sum<Y, B> value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> LiftRight<X, A, B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> LiftRight<X, A, B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, Y, A, A> LiftLeft<X, Y, A>(Transducer<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, Y, A, A> LiftLeft<X, Y, A>(Func<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(Transducer.lift(f)));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, X, Y, A, B> Pure<X, Y, A, B>(B value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(Sum<Y, B>.Right(value)));

    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, X, Y, A, B> Raise<X, Y, A, B>(Y value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(Sum<Y, B>.Left(value)));
}

/// <summary>
/// Applicative pure trait with fixed left type
/// </summary>
public interface ApplicativeSum<F, X> : FunctorSum<F, X> 
    where F : ApplicativeSum<F, X>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract K<F, X, X, A, B> Lift<A, B>(SumTransducer<X, X, A, B> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> Lift<A, B>(Func<Sum<X, A>, Sum<X, B>> f) =>
        F.Lift(SumTransducer.lift(f));
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> Lift<A, B>(Sum<X, B> value) =>    
        F.Lift(SumTransducer.constant<X, X, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> LiftRight<A, B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> LiftRight<A, B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, X, X, A, B> Pure<A, B>(B value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, X, X, A, B> Raise<A, B>(X value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Left(value)));
}

/// <summary>
/// Applicative pure trait with fixed left type and input type
/// </summary>
public interface ApplicativeSum<F, X, A> : FunctorSum<F, X, A> 
    where F : ApplicativeSum<F, X, A>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract K<F, X, X, A, B> Lift<B>(SumTransducer<X, X, A, B> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> Lift<B>(Func<Sum<X, A>, Sum<X, B>> f) =>
        F.Lift(SumTransducer.lift(f));
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> Lift<B>(Sum<X, B> value) =>    
        F.Lift(SumTransducer.constant<X, X, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, X, X, A, B> LiftRight<B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, X, X, A, B> Pure<B>(B value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, X, X, A, B> Raise<B>(X value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Left(value)));
}
