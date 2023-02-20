#nullable enable
namespace LanguageExt;

record ApplyTransducer<A, B, C>(Transducer<A, Func<B, C>> FF, Transducer<A, B> FA) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(FF, FA, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        new ApplyTransducerAsync<A, B, C>(FF.ToAsync(), FA.ToAsync());
    
    record Reduce<S>(Transducer<A, Func<B, C>> FF, Transducer<A, B> FA, Reducer<S, C> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState state, S stateValue, A value) =>
            FF.Transform(new Ap<S>(value, FA, Reducer)).Run(state, stateValue, value);

        public override ReducerAsync<S, A> ToAsync() =>
            new ApplyTransducerAsync<A, B, C>.Reduce<S>(FF.ToAsync(), FA.ToAsync(), Reducer.ToAsync());
    }
    
    record Ap<S>(A Value, Transducer<A, B> FA, Reducer<S, C> Reducer) : Reducer<S, Func<B, C>>
    {
        public override TResult<S> Run(TState state, S stateValue, Func<B, C> f) =>
            FA.Transform(new Ap2<S>(f, Reducer)).Run(state, stateValue, Value);

        public override ReducerAsync<S, Func<B, C>> ToAsync() =>
            new ApplyTransducerAsync<A, B, C>.Ap<S>(Value, FA.ToAsync(), Reducer.ToAsync());
    }
    
    record Ap2<S>(Func<B, C> F, Reducer<S, C> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState state, S stateValue, B value) =>
            TResult.Recursive(state, stateValue, F(value), Reducer);
        
        public override ReducerAsync<S, B> ToAsync() =>
            new ApplyTransducerAsync<A, B, C>.Ap2<S>(F, Reducer.ToAsync());
    }
}
