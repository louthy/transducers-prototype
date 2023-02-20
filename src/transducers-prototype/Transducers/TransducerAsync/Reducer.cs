#nullable enable
namespace LanguageExt;

/// <summary>
/// Reducer is an encapsulation of a fold operation.  It also takes a `TState` which can be
/// used to track resources allocated.
/// </summary>
/// <typeparam name="S">State type</typeparam>
/// <typeparam name="A">Value type</typeparam>
public abstract record ReducerAsync<S, A> 
{
    /// <summary>
    /// Run the reduce operation with an initial state and value
    /// </summary>
    public abstract ValueTask<TResultAsync<S>> Run(TState state, S stateValue, A value);
}
