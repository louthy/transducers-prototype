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
    public static abstract KArr<F, X, Z, A, C> BiMap<X, Y, Z, A, B, C>(
        KArr<F, X, Y, A, B> fab,
        Transducer<Y, Z> Left,
        Transducer<B, C> Right);
    
    /// <summary>
    /// Map from `X|A - >Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual KArr<F, X, Z, A, C> BiMap<X, Y, Z, A, B, C>(
        KArr<F, X, Y, A, B> fab,
        Func<Y, Z> Left,
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> Y|B` to `X|A -> Y|B -> Y|C`
    /// </summary>
    public static virtual KArr<F, X, Y, A, C> MapRight<X, Y, A, B, C>(
        KArr<F, X, Y, A, B> fab, 
        Func<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<Y>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> Y|B` to `X|A -> Y|B -> Y|C`
    /// </summary>
    public static virtual KArr<F, X, Y, A, C> MapRight<X, Y, A, B, C>(
        KArr<F, X, Y, A, B> fab, 
        Transducer<B, C> Right) =>
        F.BiMap(fab, Transducer.identity<Y>(), Right);  

    /// <summary>
    /// Map left: from `X|A -> Y|B` to `X|A -> Y|B -> Z|B`
    /// </summary>
    public static virtual KArr<F, X, Z, A, B> MapLeft<X, Y, Z, A, B>(
        KArr<F, X, Y, A, B> fab, 
        Func<Y, Z> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<B>());

    /// <summary>
    /// Map left: from `X|A -> Y|B` to `X|A -> Y|B -> Z|B`
    /// </summary>
    public static virtual KArr<F, X, Z, A, B> MapLeft<X, Y, Z, A, B>(
        KArr<F, X, Y, A, B> fab, 
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
    public static abstract KArr<F, Env, X, Env, B> BiMap<Env, A, B>(
        KArr<F, Env, X, Env, A> fab,
        Transducer<X, X> Left,
        Transducer<A, B> Right);

    /// <summary>
    /// Map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> BiMap<Env, A, B>(
        KArr<F, Env, X, Env, A> fab,
        Func<X, X> Left,
        Func<A, B> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> MapRight<Env, A, B>(
        KArr<F, Env, X, Env, A> fab, 
        Func<A, B> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> MapRight<Env, A, B>(
        KArr<F, Env, X, Env, A> fab, 
        Transducer<A, B> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Right);

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> MapLeft<Env, A>(
        KArr<F, Env, X, Env, A> fab, 
        Func<X, X> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<A>());

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> MapLeft<Env, A>(
        KArr<F, Env, X, Env, A> fab, 
        Transducer<X, X> Left) =>
        F.BiMap(fab, Left, Transducer.identity<A>());
}

/// <summary>
/// Bi-functor trait with constrained `Left`
/// </summary>
/// <typeparam name="F">Bi-functor type</typeparam>
/// <typeparam name="X">Constrained left type</typeparam>
/// <typeparam name="Env">Lower-kind bound type</typeparam>
public interface FunctorSum2<F, Env, X> where F : FunctorSum2<F, Env, X>
{
    /// <summary>
    /// Bi-map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static abstract KArr<F, Env, X, Env, B> BiMap<A, B>(
        KArr<F, Env, X, Env, A> fab,
        Transducer<X, X> Left,
        Transducer<A, B> Right);

    /// <summary>
    /// Bi-map from `X|A -> Y|B` to `X|A -> Y|B -> Z|C` 
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> BiMap<A, B>(
        KArr<F, Env, X, Env, A> fab,
        Func<X, X> Left,
        Func<A, B> Right) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.lift(Right));
    
    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> MapRight<A, B>(
        KArr<F, Env, X, Env, A> fab, 
        Func<A, B> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Transducer.lift(Right));

    /// <summary>
    /// Map right: from `X|A -> X|B` to `X|A -> X|B -> X|C`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, B> MapRight<A, B>(
        KArr<F, Env, X, Env, A> fab, 
        Transducer<A, B> Right) =>
        F.BiMap(fab, Transducer.identity<X>(), Right);

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> MapLeft<A>(
        KArr<F, Env, X, Env, A> fab, 
        Func<X, X> Left) =>
        F.BiMap(fab, Transducer.lift(Left), Transducer.identity<A>());

    /// <summary>
    /// Map left: from `X|A -> X|B` to `X|A -> X|B -> X|B`
    /// </summary>
    public static virtual KArr<F, Env, X, Env, A> MapLeft<A>(
        KArr<F, Env, X, Env, A> fab, 
        Transducer<X, X> Left) =>
        F.BiMap(fab, Left, Transducer.identity<A>());
}
