#nullable enable
namespace LanguageExt;

/// <summary>
/// Reducer is an encapsulation of a fold operation.  It also takes a `TState` which can be
/// used to track resources allocated.
/// </summary>
public record SumReducer<S, X, A>(Reducer<S, X> Left, Reducer<S, A> Right) : Reducer<S, Sum<X, A>>
{
    /// <summary>
    /// Run the reduce operation with an initial state and value
    /// </summary>
    public override TResult<S> Run(TState state, S stateValue, Sum<X, A> value) =>
        value switch
        {
            SumRight<X, A> r => Right.Run(state, stateValue, r.Value),
            SumLeft<X, A> l => Left.Run(state, stateValue, l.Value),
            _ => TResult.Complete(stateValue)
        };

    /// <summary>
    /// Lift the synchronous reducer into the asynchronous space 
    /// </summary>
    public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
        new SumReducerAsync<S, X, A>(Left.ToAsync(), Right.ToAsync());
}
