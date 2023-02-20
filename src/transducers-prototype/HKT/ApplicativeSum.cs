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
    public static abstract K<F, Sum<X, A>, Sum<Y, B>> Lift<X, Y, A, B>(Transducer<Sum<X, A>, Sum<Y, B>> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, B>> Lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> f) =>
        F.Lift(Transducer.lift(f));

    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, B>> Lift<X, Y, A, B>(Sum<Y, B> value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<X, A, B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<X, A, B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, A>> LiftLeft<X, Y, A>(Transducer<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, A>> LiftLeft<X, Y, A>(Func<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(Transducer.lift(f)));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, B>> Pure<X, Y, A, B>(B value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(Sum<Y, B>.Right(value)));

    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<Y, B>> Raise<X, Y, A, B>(Y value) =>
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
    public static abstract K<F, Sum<X, A>, Sum<X, B>> Lift<A, B>(Transducer<Sum<X, A>, Sum<X, B>> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Lift<A, B>(Func<Sum<X, A>, Sum<X, B>> f) =>
        F.Lift(Transducer.lift(f));
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Lift<A, B>(Sum<X, B> value) =>    
        F.Lift(SumTransducer.constant<X, X, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<A, B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<A, B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Pure<A, B>(B value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Raise<A, B>(X value) =>
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
    public static abstract K<F, Sum<X, A>, Sum<X, B>> Lift<B>(Transducer<Sum<X, A>, Sum<X, B>> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Lift<B>(Func<Sum<X, A>, Sum<X, B>> f) =>
        F.Lift(Transducer.lift(f));
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Lift<B>(Sum<X, B> value) =>    
        F.Lift(SumTransducer.constant<X, X, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> LiftRight<B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Pure<B>(B value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual K<F, Sum<X, A>, Sum<X, B>> Raise<B>(X value) =>
        F.Lift(SumTransducer.constant<X, X, A, B>(Sum<X, B>.Left(value)));
}
