namespace LanguageExt.HKT;

/// <summary>
/// Functor trait
/// </summary>
/// <typeparam name="F">Functor type</typeparam>
public interface Functor<F> where F : Functor<F>
{
    /// <summary>
    /// Map from `A -> B` to `A -> C` 
    /// </summary>
    public static abstract KArr<F, Unit, B> Map<A, B>(KArr<F, Unit, A> fab, Transducer<A, B> f);

    /// <summary>
    /// Map from `A -> B` to `A -> C` 
    /// </summary>
    public static virtual KArr<F, Unit, B> Map<A, B>(KArr<F, Unit, A> fab, Func<A, B> f) =>
        F.Map(fab, Transducer.lift(f));
}

/// <summary>
/// Functor trait with constrained input-type
/// </summary>
/// <typeparam name="F">Functor type</typeparam>
/// <typeparam name="A">Lower kind input type</typeparam>
public interface Functor<F, A> where F : Functor<F, A>
{
    /// <summary>
    /// Map from `A->B` to `A->C` 
    /// </summary>
    public static abstract KArr<F, A, C> Map<B, C>(KArr<F, A, B> fab, Transducer<B, C> f);
    
    /// <summary>
    /// Map from `A->B` to `A->C` 
    /// </summary>
    public static virtual KArr<F, A, C> Map<B, C>(KArr<F, A, B> fab, Func<B, C> f) =>
        F.Map(fab, Transducer.lift(f));
}
