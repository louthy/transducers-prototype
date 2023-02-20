namespace LanguageExt.HKT;

/// <summary>
/// Kind
/// </summary>
/// <typeparam name="F">Higher kind</typeparam>
/// <typeparam name="A">Lower kind input type</typeparam>
/// <typeparam name="B">Lower kind output type</typeparam>
public interface K<F, A, B>
{
    /// <summary>
    /// Transducer from `A` to `B`
    /// </summary>
    Transducer<A, B> Morphism { get; }
}

public interface K<F, X, Y, A, B> : K<F, A, B>
{
    /// <summary>
    /// Transducer from `A` to `B`
    /// </summary>
    Transducer<Sum<X, A>, Sum<Y, B>> SumMorphism { get; }
}
