#nullable enable
namespace LanguageExt;

record Invoke1Reducer<A> : Reducer<A?, A>
{
    public static readonly Reducer<A?, A> Default = new Invoke1Reducer<A>();
    
    public override TResult<A?> Run(TState state, A? stateValue, A value) => 
        TResult.Continue<A?>(value);

    public override ReducerAsync<A?, A> ToAsync() =>
        Invoke1ReducerAsync<A>.Default;
}
