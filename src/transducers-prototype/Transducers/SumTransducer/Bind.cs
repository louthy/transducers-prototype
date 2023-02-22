#nullable enable
using LanguageExt.HKT;

namespace LanguageExt;

record SumBindTransducer1<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Transducer<B, SumTransducer<X, Y, A, C>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<Sum<X, A>, Sum<Y, C>> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, C> ToSumAsync() =>
        new SumBindTransducerAsyncSync1<X, Y, A, B, C>(M.ToAsync(), F.ToAsync());
        
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Transducer<B, SumTransducer<X, Y, A, C>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new SumBindTransducerAsyncSync1<X, Y, A, B, C>.Reduce<S>(M.ToAsync(), F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Transducer<B, SumTransducer<X, Y, A, C>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, Reducer))),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, B>> ToAsync() =>
            throw new NotImplementedException(); // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO   
            //new SumBindTransducerAsyncSync1<X, Y, A, B, C>.Binder1<S>(Value, F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder2<S>(Sum<X, A> Value, Reducer<S, Sum<Y, C>> Reducer) : Reducer<S, SumTransducer<X, Y, A, C>>
    {
        public override TResult<S> Run(TState st, S s, SumTransducer<X, Y, A, C> f) =>
            f.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, SumTransducer<X, Y, A, C>> ToAsync() =>
            throw new NotImplementedException(); // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO   
            //new SumBindTransducerAsyncSync1<X, Y, A, B, C>.Binder2<S>(Value, Reducer.ToAsync());
    }
}

record SumBindTransducer2<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<Sum<X, A>, Sum<Y, C>> ToAsync() =>
        ToSumAsync();
    
    public override SumTransducerAsync<X, Y, A, C> ToSumAsync() =>
        new SumBindTransducerAsync2<X, Y, A, B, C>(M.ToAsync(), F.ToAsync());
    
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new SumBindTransducerAsync2<X, Y, A, B, C>.Reduce<S>(M.ToAsync(), F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Transducer<B, Func<Sum<X, A>, Sum<Y, C>>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, r.Value, F.Transform(new Binder2<S>(Value, Reducer))),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, B>> ToAsync() =>
            new SumBindTransducerAsync2<X, Y, A, B, C>.Binder1<S>(Value, F.ToAsync(), Reducer.ToAsync());
    }
    
    record Binder2<S>(Sum<X, A> Value, Reducer<S, Sum<Y, C>> Reducer) : Reducer<S, Func<Sum<X, A>, Sum<Y, C>>>
    {
        public override TResult<S> Run(TState st, S s, Func<Sum<X, A>, Sum<Y, C>> f) =>
            Reducer.Run(st, s, f(Value));

        public override ReducerAsync<S, Func<Sum<X, A>, Sum<Y, C>>> ToAsync() =>
            new SumBindTransducerAsync2<X, Y, A, B, C>.Binder2<S>(Value, Reducer.ToAsync());
    }
}

record SumBindTransducer3<X, Y, A, B, C>(
    Transducer<Sum<X, A>, Sum<Y, B>> M, 
    Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F) : 
    SumTransducer<X, Y, A, C>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, C>> reduce) =>
        new Reduce<S>(M, F, reduce);

    public override TransducerAsync<Sum<X, A>, Sum<Y, C>> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, C> ToSumAsync() =>
        new SumBindTransducerAsync3<X, Y, A, B, C>(M.ToAsync(), x => F(x).ToAsync());
    
    record Reduce<S>(
        Transducer<Sum<X, A>, Sum<Y, B>> M, 
        Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F, Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> value) =>
            M.Transform(new Binder1<S>(value, F, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new SumBindTransducerAsync3<X, Y, A, B, C>.Reduce<S>(M.ToAsync(), x => F(x).ToAsync(), Reducer.ToAsync());
    }
    
    record Binder1<S>(
        Sum<X, A> Value, 
        Func<B, Transducer<Sum<X, A>, Sum<Y, C>>> F, 
        Reducer<S, Sum<Y, C>> Reducer) 
        : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r =>
                    TResult.Recursive(st, s, Value, F(r.Value).Transform(Reducer)),

                SumLeft<Y, B> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, B>> ToAsync() =>
            new SumBindTransducerAsync3<X, Y, A, B, C>.Binder1<S>(Value, x => F(x).ToAsync(), Reducer.ToAsync());
    }
}
