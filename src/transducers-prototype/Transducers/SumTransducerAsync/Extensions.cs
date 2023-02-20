namespace LanguageExt;

public static partial class Transducer
{
    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static TransducerAsync<Sum<X, A>, Sum<Y, C>> Apply<X, Y, A, B, C>(
        this TransducerAsync<Sum<X, A>, Sum<Y, Func<B, C>>> ff,
        TransducerAsync<Sum<X, A>, Sum<Y, B>> fa) =>
        new SumApplyTransducerAsync<X, Y, A, B, C>(ff, fa);
}