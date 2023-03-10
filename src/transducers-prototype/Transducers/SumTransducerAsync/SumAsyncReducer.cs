#nullable enable
namespace LanguageExt;

/// <summary>
/// Reducer is an encapsulation of a fold operation.  It also takes a `TState` which can be
/// used to track resources allocated.
/// </summary>
public record SumReducerAsync<S, X, A>(ReducerAsync<S, X> Left, ReducerAsync<S, A> Right)
    : ReducerAsync<S, Sum<X, A>>
{
    /// <summary>
    /// Run the reduce operation with an initial state and value
    /// </summary>
    public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, Sum<X, A> value) =>
        value switch
        {
            SumRight<X, A> r => Right.Run(state, stateValue, r.Value),
            SumLeft<X, A> l => Left.Run(state, stateValue, l.Value),
            _ => new(TResultAsync.Complete(stateValue))
        };
}
