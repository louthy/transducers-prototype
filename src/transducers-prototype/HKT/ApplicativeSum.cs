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
    public static abstract KArr<F, X, Y, A, B> Lift<X, Y, A, B>(SumTransducer<X, Y, A, B> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, X, Y, A, B> Lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> f) =>
        F.Lift(SumTransducer.lift(f));

    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual KArr<F, X, Y, A, B> Lift<X, Y, A, B>(Sum<Y, B> value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(value));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, X, X, A, B> LiftRight<X, A, B>(Transducer<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, X, X, A, B> LiftRight<X, A, B>(Func<A, B> f) =>
        F.Lift(SumTransducer.mapRight<X, A, B>(Transducer.lift(f)));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, X, Y, A, A> LiftLeft<X, Y, A>(Transducer<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, X, Y, A, A> LiftLeft<X, Y, A>(Func<X, Y> f) =>
        F.Lift(SumTransducer.mapLeft<X, Y, A>(Transducer.lift(f)));

    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, X, Y, A, B> Pure<X, Y, A, B>(B value) =>
        F.Lift(SumTransducer.constant<X, Y, A, B>(Sum<Y, B>.Right(value)));

    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual KArr<F, X, Y, A, B> Raise<X, Y, A, B>(Y value) =>
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
    public static abstract KArr<F, Env, X, Env, A> Lift<Env, A>(SumTransducer<Env, X, Env, A> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Lift<Env, A>(Func<Sum<Env, Env>, Sum<X, A>> f) =>
        F.Lift(SumTransducer.lift(f));

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Lift<Env, A>(Func<Env, Sum<X, A>> f) =>
        F.Lift((Sum<Env, Env> e) => e switch
        {
            SumRight<Env, Env> r => f(r.Value),
            SumLeft<Env, Env> l => f(l.Value),
            _ => throw new NotSupportedException()
        });
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Lift<Env, A>(Sum<X, A> value) =>    
        F.Lift(SumTransducer.constant<Env, X, Env, A>(value));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Pure<Env, A>(A value) =>
        F.Lift(SumTransducer.constant<Env, X, Env, A>(Sum<X, A>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Raise<Env, A>(X value) =>
        F.Lift(SumTransducer.constant<Env, X, Env, A>(Sum<X, A>.Left(value)));
}

/// <summary>
/// Applicative pure trait with fixed left type and input type
/// </summary>
public interface ApplicativeSum<F, Env, X> : FunctorSum<F, Env, X> 
    where F : ApplicativeSum<F, Env, X>
{
    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static abstract KArr<F, Env, X, Env, A> Lift<A>(SumTransducer<Env, X, Env, A> f);

    /// <summary>
    /// Lift transducer into applicative
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Lift<A>(Func<Sum<Env, Env>, Sum<X, A>> f) =>
        F.Lift(SumTransducer.lift(f));
    
    /// <summary>
    /// Lift sum-type into applicative
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Lift<A>(Sum<X, A> value) =>    
        F.Lift(SumTransducer.constant<Env, X, Env, A>(value));
    
    /// <summary>
    /// Pure constructor
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> Pure<A>(A value) =>
        F.Lift(SumTransducer.constant<Env, X, Env, A>(Sum<X, A>.Right(value)));
    
    /// <summary>
    /// Left constructor
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> Raise<B>(X value) =>
        F.Lift(SumTransducer.constant<Env, X, Env, B>(Sum<X, B>.Left(value)));
}
