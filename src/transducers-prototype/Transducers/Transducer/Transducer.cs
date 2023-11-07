﻿#nullable enable
using LanguageExt.HKT;

namespace LanguageExt;

/// <summary>
/// Transducers are composable algorithmic transformations. They are independent from the context of their input and
/// output sources and specify only the essence of the transformation in terms of an individual element. Because
/// transducers are decoupled from input or output sources, they can be used in many different processes -
/// collections, streams, channels, observables, etc. Transducers compose directly, without awareness of input or
/// creation of intermediate aggregates.
/// </summary>
/// <typeparam name="A">Input value type</typeparam>
/// <typeparam name="B">Output value type</typeparam>
public interface Transducer<A, B> : KArr<Any, A, B>
{
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    Reducer<S, A> Transform<S>(Reducer<S, B> reduce);

    /// <summary>
    /// Compose this transducer and the next
    /// </summary>
    /// <param name="next">Next transducer</param>
    /// <returns>Composition of the two transducers</returns>
    //Transducer<A, C> Compose<C>(Transducer<B, C> next);
}
