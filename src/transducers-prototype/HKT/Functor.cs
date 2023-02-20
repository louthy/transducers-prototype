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
    public static abstract K<F, A, C> Map<A, B, C>(K<F, A, B> fab, Transducer<B, C> f);

    /// <summary>
    /// Map from `A -> B` to `A -> C` 
    /// </summary>
    public static K<F, A, C> Map<A, B, C>(K<F, A, B> fab, Func<B, C> f) =>
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
    public static abstract K<F, A, C> Map<B, C>(K<F, A, B> fab, Transducer<B, C> f);
    
    /// <summary>
    /// Map from `A->B` to `A->C` 
    /// </summary>
    public static K<F, A, C> Map<B, C>(K<F, A, B> fab, Func<B, C> f) =>
        F.Map(fab, Transducer.lift(f));
}
