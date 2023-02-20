#nullable enable
namespace LanguageExt;

record ApplyTransducerAsync<A, B, C>(TransducerAsync<A, Func<B, C>> FF, TransducerAsync<A, B> FA) : 
    TransducerAsync<A, C>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, C> reduce) =>
        new Reduce<S>(FF, FA, reduce);
    
    internal record Reduce<S>(TransducerAsync<A, Func<B, C>> FF, TransducerAsync<A, B> FA, ReducerAsync<S, C> Reducer) : 
        ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, A value) =>
            FF.Transform(new Ap<S>(value, FA, Reducer)).Run(state, stateValue, value);
    }
    
    internal record Ap<S>(A Value, TransducerAsync<A, B> FA, ReducerAsync<S, C> Reducer) : ReducerAsync<S, Func<B, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, Func<B, C> f) =>
            FA.Transform(new Ap2<S>(f, Reducer)).Run(state, stateValue, Value);
    }
    
    internal record Ap2<S>(Func<B, C> F, ReducerAsync<S, C> Reducer) : ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState state, S stateValue, B value) =>
            TResultAsync.Recursive(state, stateValue, F(value), Reducer);
    }
}
