namespace LanguageExt;

record SumApplyTransducer<X, Y, A, B, C>(
        Transducer<Sum<X, A>, Sum<Y, Func<B, C>>> FF,
        Transducer<Sum<X, A>, Sum<Y, B>> FA) 
    : Transducer<Sum<X, A>, Sum<Y, C>>
{
    /// <summary>
    /// Transform the transducer using the reducer provided 
    /// </summary>
    /// <param name="reduce">Reducer</param>
    /// <typeparam name="S">State type</typeparam>
    /// <returns>Reducer that captures the transformation of the `Transducer` and the provided reducer</returns>
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(FF, FA, reduce);

    /// <summary>
    /// Lift the synchronous sum-transducer into the asynchronous space 
    /// </summary>
    public override TransducerAsync<Sum<X, A>, Sum<Y, C>> ToAsync() =>
        new SumApplyTransducerAsync<X, Y, A, B, C>(FF.ToAsync(), FA.ToAsync());
    
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, Func<B, C>>> FF,
        Transducer<Sum<X, A>, Sum<Y, B>> FA, 
        Reducer<S, Sum<Y, C>> Reducer) : 
        Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> v) =>
            FF.Transform(new ApFF<S>(v, FA, Reducer)).Run(st, s, v);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new SumApplyTransducerAsync<X, Y, A, B, C>.Reduce<S>(FF.ToAsync(), FA.ToAsync(), Reducer.ToAsync());
    }
    
    record ApFF<S>(
        Sum<X, A> Value,
        Transducer<Sum<X, A>, Sum<Y, B>> FA,
        Reducer<S, Sum<Y, C>> Reducer) :
        Reducer<S, Sum<Y, Func<B, C>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, Func<B, C>> sf) =>
            sf switch
            {
                SumRight<Y, Func<B, C>> f =>
                    TResult.Recursive(st, s, Value, FA.Transform(new ApFA<S>(f.Value, Reducer))),

                SumLeft<Y, Func<B, C>> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, Func<B, C>>> ToAsync() =>
            new SumApplyTransducerAsync<X, Y, A, B, C>.ApFF<S>(Value, FA.ToAsync(), Reducer.ToAsync());
    }
        
    record ApFA<S>(Func<B, C> FF, Reducer<S, Sum<Y, C>> Reducer) : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r => Reducer.Run(st, s, Sum<Y, C>.Right(FF(r.Value))),
                SumLeft<Y, B> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, B>> ToAsync() =>
            new SumApplyTransducerAsync<X, Y, A, B, C>.ApFA<S>(FF, Reducer.ToAsync());
    }
}