namespace LanguageExt;

public static partial class TransducerAsync
{
    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> Flatten<A, B>(this TransducerAsync<A, TransducerAsync<A, B>> ff) =>
        new FlattenTransducerAsync1<A, B>(ff);

    /// <summary>
    /// Take nested transducers and flatten them
    /// </summary>
    /// <param name="ff">Nested transducers</param>
    /// <returns>Flattened transducers</returns>
    public static TransducerAsync<A, B> Flatten<A, B>(this TransducerAsync<A, TransducerAsync<Unit, B>> ff) =>
        new FlattenTransducerAsync2<A, B>(ff);

    /// <summary>
    /// Applicative apply
    /// </summary>
    /// <remarks>
    /// Gets a lifted function and a lifted argument, invokes the function with the argument and re-lifts the result.
    /// </remarks>
    /// <returns>Result of applying the function to the argument</returns>
    public static TransducerAsync<A, C> Apply<A, B, C>(
        this TransducerAsync<A, Func<B, C>> ff,
        TransducerAsync<A, B> fa) =>
        new ApplyTransducerAsync<A, B, C>(ff, fa);
}