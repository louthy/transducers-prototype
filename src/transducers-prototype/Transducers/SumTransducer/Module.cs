namespace LanguageExt;

public static class SumTransducer
{
    /// <summary>
    /// Make a sum-transducer from a pair of transducers
    /// </summary>
    /// <param name="left">Left transducer</param>
    /// <param name="right">Right transducer</param>
    /// <returns>Sum transducer</returns>
    public static Transducer<Sum<X, A>, Sum<Y, B>> make<X, Y, A, B>(
        Transducer<X, Sum<Y, B>> left, 
        Transducer<A, Sum<Y, B>> right) =>
        new SumTransducer<X, Y, A, B>(left, right);

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static Transducer<Sum<Unit, Unit>, Sum<X, A>> Pure<X, A>(Sum<X, A> value) =>
        constant<Unit, X, Unit, A>(value);

    /// <summary>
    /// Identity transducer
    /// </summary>
    public static Transducer<Sum<X, A>, Sum<X, A>> identity<X, A>() =>
        make(Transducer.lift<X, Sum<X, A>>(x => Sum<X, A>.Left(x)),
             Transducer.lift<A, Sum<X, A>>(a => Sum<X, A>.Right(a)));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <remarks>This transducer effectively ignores the input value and yields the provided constant sum
    /// value.</remarks>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static Transducer<Sum<X, A>, Sum<Y, B>> constant<X, Y, A, B>(Sum<Y, B> value) =>
        make(Transducer.constant<X, Sum<Y, B>>(value), 
            Transducer.constant<A, Sum<Y, B>>(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static Transducer<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(Func<Sum<X, A>> value) =>
        make(Transducer.lift(value), 
             Transducer.lift(value));

    /// <summary>
    /// Lift a constant sum-value into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `Unit | Unit` sum is applied</returns>
    public static Transducer<Sum<Unit, Unit>, Sum<X, A>> lift<X, A>(Func<TResult<Sum<X, A>>> value) =>
        make(Transducer.lift(value), 
             Transducer.lift(value));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static Transducer<Sum<X, A>, Sum<Y, B>> lift<X, Y, A, B>(Func<Sum<X, A>, Sum<Y, B>> value) =>
        make(Transducer.lift((X x) => value(Sum<X, A>.Left(x))),
             Transducer.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Lift a function into the sum-transducer
    /// </summary>
    /// <param name="value">Value to lift</param>
    /// <returns>Transducer that yields the provided `Sum` value when a `X | A` sum is applied</returns>
    public static Transducer<Sum<X, A>, Sum<Y, B>> lift<X, Y, A, B>(Func<Sum<X, A>, TResult<Sum<Y, B>>> value) =>
        make(Transducer.lift((X x) => value(Sum<X, A>.Left(x))),
             Transducer.lift((A a) => value(Sum<X, A>.Right(a))));

    /// <summary>
    /// Bi-mapping transducers
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static Transducer<Sum<X, A>, Sum<Y, B>> bimap<X, Y, A, B>(
        Transducer<X, Y> Left,
        Transducer<A, B> Right) =>
        make(Left.Map(Sum<Y, B>.Left), Right.Map(Sum<Y, B>.Right));

    /// <summary>
    /// Map left transducer
    /// </summary>
    /// <param name="Left">Left mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static Transducer<Sum<X, A>, Sum<Y, A>> mapLeft<X, Y, A>(Transducer<X, Y> Left) =>
        bimap(Left, Transducer.identity<A>());

    /// <summary>
    /// Map right transducer
    /// </summary>
    /// <param name="Right">Right mapping transducer</param>
    /// <returns>Bi-functor transducer</returns>
    public static Transducer<Sum<X, A>, Sum<X, B>> mapRight<X, A, B>(Transducer<A, B> Right) =>
        bimap(Transducer.identity<X>(), Right);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static Transducer<Sum<X, A>, Sum<Y, C>> apply<X, Y, A, B, C>(
        Transducer<Sum<X, A>, Sum<Y, Func<B, C>>> ff,
        Transducer<Sum<X, A>, Sum<Y, B>> fa) =>
        new SumApplyTransducer<X, Y, A, B, C>(ff, fa);
}