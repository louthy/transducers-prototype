#nullable enable
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
public abstract record TransducerAsync<A, B>
{
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    public abstract ReducerAsync<S, A> Transform<S>(ReducerAsync<S, B> reduce);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public TransducerAsync<A, C> Compose<C>(TransducerAsync<B, C> g) =>
        new ComposeTransducerAsync<A, B, C>(this, g);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public TransducerAsync<A, D> Compose<C, D>(TransducerAsync<B, C> g, TransducerAsync<C, D> h) =>
        new ComposeTransducerAsync<A, B, C, D>(this, g, h);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public TransducerAsync<A, E> Compose<C, D, E>(
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h, 
        TransducerAsync<D, E> i) =>
        new ComposeTransducerAsync<A, B, C, D, E>(this, g, h, i);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public TransducerAsync<A, F> Compose<C, D, E, F>(
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h, 
        TransducerAsync<D, E> i, 
        TransducerAsync<E, F> j) =>
        new ComposeTransducerAsync<A, B, C, D, E, F>(this, g, h, i, j);

    /// <summary>
    /// Transducer composition.  The output of one transducer is fed as the input to the next.
    ///
    /// Resulting im a single transducer that captures the composition
    /// </summary>
    /// <returns>Transducer that captures the composition</returns>
    public TransducerAsync<A, G> Compose<C, D, E, F, G>(
        TransducerAsync<B, C> g, 
        TransducerAsync<C, D> h, 
        TransducerAsync<D, E> i, 
        TransducerAsync<E, F> j, 
        TransducerAsync<F, G> k) =>
        new ComposeTransducerAsync<A, B, C, D, E, F, G>(this, g, h, i, j, k);

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
    public async ValueTask<TResult<B>> Invoke1(A value)
    {
        var tb = await Invoke(value, default, Invoke1ReducerAsync<B>.Default).ConfigureAwait(false);
        return tb.Bind(static b => b is null ? TResult.None<B>() : TResult.Complete<B>(b));
    }

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
    public async ValueTask<TResult<S>> Invoke<S>(A value, S initialState, ReducerAsync<S, B> reducer)
    {
        var st = new TState();

        try
        {
            var s = initialState;
            var tf = Transform(reducer);
            var tr = await tf.Run(st, s, value).ConfigureAwait(false);

            while (true)
            {
                switch (tr)
                {
                    case TRecursiveAsync<S> r:
                        tr = await r.Run().ConfigureAwait(false);
                        break;

                    case TContinueAsync<S> {Value: not null} r:
                        return TResult.Complete<S>(r.Value);

                    case TCompleteAsync<S> {Value: not null} r:
                        return TResult.Complete<S>(r.Value);

                    case TCancelledAsync<S>:
                        return TResult.Cancel<S>();

                    case TFailAsync<S> r:
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
