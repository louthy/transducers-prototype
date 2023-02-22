namespace LanguageExt.HKT;

/// <summary>
/// Sum-functor trait
/// </summary>
/// <typeparam name="F">Sum-functor type</typeparam>
public interface FunctorSum<F> where F : FunctorSum<F>
{
    /// <summary>
    /// Map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static abstract K<F, X, Z, A, C> BiMap<X, Y, Z, A, B, C>(
        K<F, X, Y, A, B> fab,
        Transducer<Y, Z> Left,
        Transducer<B, C> Right);
    
    /// <summary>
    /// Map from `X|A - >Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual K<F, X, Z, A, C> BiMap<X, Y, Z, A, B, C>(
        K<F, X, Y, A, B> fab,
        Func<Y, Z> Left,
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> Y|B` to `X|A -> Y|B -> Y|C`
    /// </summary>
    public static virtual K<F, X, Y, A, C> MapRight<X, Y, A, B, C>(
        K<F, X, Y, A, B> fab, 
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<Y>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> Y|B` to `X|A -> Y|B -> Y|C`
    /// </summary>
    public static virtual K<F, X, Y, A, C> MapRight<X, Y, A, B, C>(
        K<F, X, Y, A, B> fab, 
        Transducer<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<Y>(), Right);  

    /// <summary>
    /// Map left: from `X|A -> Y|B` to `X|A -> Y|B -> Z|B`
    /// </summary>
    public static virtual K<F, X, Z, A, B> MapLeft<X, Y, Z, A, B>(
        K<F, X, Y, A, B> fab, 
        Func<Y, Z> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<B>());

    /// <summary>
    /// Map left: from `X|A -> Y|B` to `X|A -> Y|B -> Z|B`
    /// </summary>
    public static virtual K<F, X, Z, A, B> MapLeft<X, Y, Z, A, B>(
        K<F, X, Y, A, B> fab, 
        Transducer<Y, Z> Left) =>
        F.BiMap(fab, Left, Transducer.identity<B>());
}

/// <summary>
/// Bi-functor trait with constrained `Left`
/// </summary>
/// <typeparam name="F">Bi-functor type</typeparam>
/// <typeparam name="X">Constrained left type</typeparam>
public interface FunctorSum<F, X> where F : FunctorSum<F, X>
{
    /// <summary>
    /// Map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static abstract K<F, X, X, A, C> BiMap<A, B, C>(
        K<F, X, X, A, B> fab,
        Transducer<X, X> Left,
        Transducer<B, C> Right);

    /// <summary>
    /// Map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual K<F, X, X, A, C> BiMap<A, B, C>(
        K<F, X, X, A, B> fab,
        Func<X, X> Left,
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual K<F, X, X, A, C> MapRight<A, B, C>(
        K<F, X, X, A, B> fab, 
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual K<F, X, X, A, C> MapRight<A, B, C>(
        K<F, X, X, A, B> fab, 
        Transducer<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Right);

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual K<F, X, X, A, B> MapLeft<A, B>(
        K<F, X, X, A, B> fab, 
        Func<X, X> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<B>());

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual K<F, X, X, A, B> MapLeft<A, B>(
        K<F, X, X, A, B> fab, 
        Transducer<X, X> Left) =>
        F.BiMap(fab, Left, Transducer.identity<B>());
}

/// <summary>
/// Bi-functor trait with constrained `Left`
/// </summary>
/// <typeparam name="F">Bi-functor type</typeparam>
/// <typeparam name="X">Constrained left type</typeparam>
/// <typeparam name="A">Lower-kind bound type</typeparam>
public interface FunctorSum<F, X, A> where F : FunctorSum<F, X, A>
{
    /// <summary>
    /// Bi-map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static abstract K<F, X, X, A, C> BiMap<B, C>(
        K<F, X, X, A, B> fab,
        Transducer<X, X> Left,
        Transducer<B, C> Right);

    /// <summary>
    /// Bi-map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual K<F, X, X, A, C> BiMap<B, C>(
        K<F, X, X, A, B> fab,
        Func<X, X> Left,
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual K<F, X, X, A, C> MapRight<B, C>(
        K<F, X, X, A, B> fab, 
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual K<F, X, X, A, C> MapRight<B, C>(
        K<F, X, X, A, B> fab, 
        Transducer<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Right);

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual K<F, X, X, A, B> MapLeft<B>(
        K<F, X, X, A, B> fab, 
        Func<X, X> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<B>());

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual K<F, X, X, A, B> MapLeft<B>(
        K<F, X, X, A, B> fab, 
        Transducer<X, X> Left) =>
        F.BiMap(fab, Left, Transducer.identity<B>());
}
