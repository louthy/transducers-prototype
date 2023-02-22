#nullable enable
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
public abstract record Transducer<A, B> : K<Any, A, B>
{
    /// <summary>
    /// Self access
    /// </summary>
    public Transducer<A, B> Morphism => 
        this;
    
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    public abstract Reducer<S, A> Transform<S>(Reducer<S, B> reduce);

    /// <summary>
    /// Lifts this `Transducer` into a `TransducerAsync` space.     
    /// </summary>
    /// <remarks>
    /// This allows synchronous transducers to be used alongside asynchronous transducers  
    /// </remarks>
    /// <returns>Asynchronous transducer version of this transducer </returns>
    public abstract TransducerAsync<A, B> ToAsync();

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public Transducer<A, C> Compose<C>(Transducer<B, C> g) =>
        new ComposeTransducer<A, B, C>(this, g);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public Transducer<A, D> Compose<C, D>(Transducer<B, C> g, Transducer<C, D> h) =>
        new ComposeTransducer<A, B, C, D>(this, g, h);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public Transducer<A, E> Compose<C, D, E>(Transducer<B, C> g, Transducer<C, D> h, Transducer<D, E> i) =>
        new ComposeTransducer<A, B, C, D, E>(this, g, h, i);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public Transducer<A, F> Compose<C, D, E, F>(
        Transducer<B, C> g, 
        Transducer<C, D> h, 
        Transducer<D, E> i, 
        Transducer<E, F> j) =>
        new ComposeTransducer<A, B, C, D, E, F>(this, g, h, i, j);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public Transducer<A, G> Compose<C, D, E, F, G>(
        Transducer<B, C> g, 
        Transducer<C, D> h, 
        Transducer<D, E> i, 
        Transducer<E, F> j, 
        Transducer<F, G> k) =>
        new ComposeTransducer<A, B, C, D, E, F, G>(this, g, h, i, j, k);

    /// <summary>
    /// Invoke the transducer, reducing to a single value only
    /// </summary>
    /// <param name="value">Value to use as the argument to the transducer</param>
    /// <returns>
    /// If the transducer yields multiple values then it will return the last value in a `TResult.Complete`.
    /// If the transducer yields zero values then it will return `TResult.None`. 
    /// If the transducer throws an exception or yields an `Error`, then it will return `TResult.Fail`.
    /// If the transducer is cancelled, then it will return `TResult.Cancelled`. 
    /// </returns>
    public TResult<B> Invoke1(A value) =>
        Invoke(value, default, Invoke1Reducer<B>.Default)
            .Bind(static b => b is null ? TResult.None<B>() : TResult.Complete<B>(b));
    
    /// <summary>
    /// Invoke the transducer, transforming the input value and finally reducing the output  with
    /// the `Reducer` provided
    /// </summary>
    /// <param name="value">Value to use as the argument to the transducer</param>
    /// <param name="initialState">Starting state</param>
    /// <param name="reducer">Value to use as the argument to the transducer</param>
    /// <returns>
    /// If the transducer yields multiple values then it will return the last value in a `TResult.Complete`.
    /// If the transducer yields zero values then it will return `TResult.None`. 
    /// If the transducer throws an exception or yields an `Error`, then it will return `TResult.Fail`.
    /// If the transducer is cancelled, then it will return `TResult.Cancelled`. 
    /// </returns>
    public TResult<S> Invoke<S>(A value, S initialState, Reducer<S, B> reducer)
    {
        var st = new TState();

        try
        {
            var s = initialState;
            var tf = Transform(reducer);
            var tr = tf.Run(st, s, value);

            while (true)
            {
                switch (tr)
                {
                    case TRecursive<S> r:
                        tr = r.Run();
                        break;

                    case TContinue<S> {Value: not null} r:
                        return TResult.Complete<S>(r.Value);

                    case TComplete<S> {Value: not null} r:
                        return TResult.Complete<S>(r.Value);

                    case TCancelled<S>:
                        return TResult.Cancel<S>();

                    case TFail<S> r:
                        return TResult.Fail<S>(r.Error);

                    default:
                        return TResult.None<S>();
                }
            }
        }
        catch (Exception e)
        {
            return TResult.Fail<S>(e);
        }
        finally
        {
            st.Dispose();
        }
    }    

}
