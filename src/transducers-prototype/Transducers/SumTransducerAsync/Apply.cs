namespace LanguageExt;

record SumApplyTransducerAsync<X, Y, A, B, C>(
        TransducerAsync<Sum<X, A>, Sum<Y, Func<B, C>>> FF,
        TransducerAsync<Sum<X, A>, Sum<Y, B>> FA) 
    : SumTransducerAsync<X, Y, A, C>
{
    /// <summary>
    /// Transform the transducerAsync using the reducerAsync provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>ReducerAsync that captures the transformation of the `TransducerAsync` and the provided reducer</returns>
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(FF, FA, reduce);
    
    internal record Reduce<S>(
        TransducerAsync<Sum<X, A>, Sum<Y, Func<B, C>>> FF,
        TransducerAsync<Sum<X, A>, Sum<Y, B>> FA, 
        ReducerAsync<S, Sum<Y, C>> Reducer) : 
        ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> v) =>
            FF.Transform(new ApFF<S>(v, FA, Reducer)).Run(st, s, v);
    }
    
    internal record ApFF<S>(
        Sum<X, A> Value,
        TransducerAsync<Sum<X, A>, Sum<Y, B>> FA,
        ReducerAsync<S, Sum<Y, C>> Reducer) :
        ReducerAsync<S, Sum<Y, Func<B, C>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, Func<B, C>> sf) =>
            sf switch
            {
                SumRight<Y, Func<B, C>> f =>
                    new(TResultAsync.Recursive(st, s, Value, FA.Transform(new ApFA<S>(f.Value, Reducer)))),

                SumLeft<Y, Func<B, C>> =>
                    new(TResultAsync.Continue(s)),

                _ =>
                    new(TResultAsync.Complete(s))
            };
    }
        
    internal record ApFA<S>(Func<B, C> FF, ReducerAsync<S, Sum<Y, C>> Reducer) : ReducerAsync<S, Sum<Y, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r => Reducer.Run(st, s, Sum<Y, C>.Right(FF(r.Value))),
                SumLeft<Y, B> => new(TResultAsync.Continue(s)),
                _ => new(TResultAsync.Complete(s))
            };
    }
}