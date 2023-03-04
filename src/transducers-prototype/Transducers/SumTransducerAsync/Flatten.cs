#nullable enable
namespace LanguageExt;

record FlattenSumTransducerAsync1<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, A, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, A, B>> FF, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, ReducerAsync)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<Y, SumTransducerAsync<X, Y, A, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducerAsync<X, Y, A, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducerAsync<X, Y, A, B>> f => f.Value.Transform(ReducerAsync).Run(st, s, Value),
                SumLeft<Y, SumTransducerAsync<X, Y, A, B>> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }
}

record FlattenSumTransducerAsync2<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, Unit, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);
    
    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducerAsync<X, Y, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, ReducerAsync)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<Y, SumTransducerAsync<X, Y, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducerAsync<X, Y, Unit, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducerAsync<X, Y, A, B>> f => f.Value.Transform(ReducerAsync).Run(st, s, Value),
                SumLeft<Y, SumTransducerAsync<X, Y, A, B>> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }
}


record FlattenSumTransducerAsyncSync1<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducer<X, Y, A, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducer<X, Y, A, B>> FF, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, ReducerAsync)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<Y, SumTransducer<X, Y, A, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducer<X, Y, A, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducer<X, Y, A, B>> f => f.Value.ToSumAsync().Transform(ReducerAsync).Run(st, s, Value),
                SumLeft<Y, SumTransducer<X, Y, A, B>> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }
}

record FlattenSumTransducerAsyncSync2<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducer<X, Y, Unit, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);
    
    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducer<X, Y, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, ReducerAsync)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> ReducerAsync) : ReducerAsync<S, Sum<Y, SumTransducer<X, Y, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducer<X, Y, Unit, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducer<X, Y, A, B>> f => f.Value.ToSumAsync().Transform(ReducerAsync).Run(st, s, Value),
                SumLeft<Y, SumTransducer<X, Y, A, B>> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }
}

record FlattenSumTransducerAsync3<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducerAsync<Unit, Unit, Unit, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducerAsync<Unit, Unit, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Y, SumTransducerAsync<Unit, Unit, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducerAsync<Unit, Unit, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducerAsync<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducerAsync<Unit, Unit, A, B>>, _) => TResultAsync.Continue(s),
                (_, SumLeft<X, A>) => TResultAsync.Continue(s),
                
                _ => TResultAsync.Complete(s)
            };
    }
    
    record Reduce2<S>(A Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }    
}

record FlattenSumTransducerAsyncSync3<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> FF) : SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Y, SumTransducer<Unit, Unit, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducer<Unit, Unit, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducer<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.ToSumAsync().Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducer<Unit, Unit, A, B>>, _) => TResultAsync.Continue(s),
                (_, SumLeft<X, A>) => TResultAsync.Continue(s),
                
                _ => TResultAsync.Complete(s)
            };
    }
    
    internal record Reduce2<S>(A Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }    
}

record FlattenSumTransducerAsyncSync4<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducer<Unit, Y, Unit, B>> FF) : 
    SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);
    
    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducer<Unit, Y, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> Reducer) : 
        ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Y, SumTransducer<Unit, Y, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducer<Unit, Y, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducer<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.ToSumAsync().Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducer<Unit, Unit, A, B>>, _) => TResultAsync.Continue(s),
                (_, SumLeft<X, A>) => TResultAsync.Continue(s),
                
                _ => TResultAsync.Complete(s)
            };
    }
    
    internal record Reduce2<S>(A Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }    
}

record FlattenSumTransducerAsync4<X, Y, A, B>(SumTransducerAsync<X, Y, A, SumTransducerAsync<Unit, Y, Unit, B>> FF) : 
    SumTransducerAsync<X, Y, A, B>
{
    public override ReducerAsync<S, Sum<X, A>> Transform<S>(ReducerAsync<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);
    
    internal record Reduce<S>(SumTransducerAsync<X, Y, A, SumTransducerAsync<Unit, Y, Unit, B>> FF, ReducerAsync<S, Sum<Y, B>> Reducer) : 
        ReducerAsync<S, Sum<X, A>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);
    }    
    
    internal record Reduce1<S>(Sum<X, A> Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Y, SumTransducerAsync<Unit, Y, Unit, B>>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Y, SumTransducerAsync<Unit, Y, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducerAsync<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducerAsync<Unit, Unit, A, B>>, _) => TResultAsync.Continue(s),
                (_, SumLeft<X, A>) => TResultAsync.Continue(s),
                
                _ => TResultAsync.Complete(s)
            };
    }
    
    internal record Reduce2<S>(A Value, ReducerAsync<S, Sum<Y, B>> Reducer) : ReducerAsync<S, Sum<Unit, B>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResultAsync.Continue(s),
                _ => TResultAsync.Complete(s)
            };
    }    
}
