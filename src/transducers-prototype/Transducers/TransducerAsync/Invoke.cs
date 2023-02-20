#nullable enable
namespace LanguageExt;

record Invoke1ReducerAsync<A> : ReducerAsync<A?, A>
{
    public static readonly ReducerAsync<A?, A> Default = new Invoke1ReducerAsync<A>();
    
    public override ValueTask<TResultAsync<A?>> Run(TState state, A? stateValue, A value) => 
        new(TResultAsync.Continue<A?>(value));
}
