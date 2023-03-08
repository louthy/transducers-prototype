#nullable enable
namespace LanguageExt;

record FlattenSumTransducer1<X, Y, A, B>(SumTransducer<X, Y, A, SumTransducer<X, Y, A, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        new FlattenSumTransducerAsyncSync1<X, Y, A, B>(FF.ToSumAsync());
    
    record Reduce<S>(SumTransducer<X, Y, A, SumTransducer<X, Y, A, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new FlattenSumTransducerAsyncSync1<X, Y, A, B>.Reduce<S>(FF.ToSumAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(Sum<X, A> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, SumTransducer<X, Y, A, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, SumTransducer<X, Y, A, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducer<X, Y, A, B>> f => f.Value.Transform(Reducer).Run(st, s, Value),
                SumLeft<Y, SumTransducer<X, Y, A, B>> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, SumTransducer<X, Y, A, B>>> ToAsync() =>
            new FlattenSumTransducerAsyncSync1<X, Y, A, B>.Reduce1<S>(Value, Reducer.ToAsync());
    }
}

record FlattenSumTransducer2<X, Y, A, B>(SumTransducer<X, Y, A, SumTransducer<X, Y, Unit, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        new FlattenSumTransducerAsyncSync2<X, Y, A, B>(FF.ToSumAsync());
    
    record Reduce<S>(SumTransducer<X, Y, A, SumTransducer<X, Y, Unit, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new FlattenSumTransducerAsyncSync2<X, Y, A, B>.Reduce<S>(FF.ToSumAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(Sum<X, A> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, SumTransducer<X, Y, Unit, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, SumTransducer<X, Y, Unit, B>> sf) =>
            sf switch
            {
                SumRight<Y, SumTransducer<X, Y, A, B>> f => f.Value.Transform(Reducer).Run(st, s, Value),
                SumLeft<Y, SumTransducer<X, Y, A, B>> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, SumTransducer<X, Y, Unit, B>>> ToAsync() =>
            new FlattenSumTransducerAsyncSync2<X, Y, A, B>.Reduce1<S>(Value, Reducer.ToAsync());
    }
}

record FlattenSumTransducer3<X, Y, A, B>(SumTransducer<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        new FlattenSumTransducerAsyncSync3<X, Y, A, B>(FF.ToSumAsync());
    
    record Reduce<S>(SumTransducer<X, Y, A, SumTransducer<Unit, Unit, Unit, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new FlattenSumTransducerAsyncSync3<X, Y, A, B>.Reduce<S>(FF.ToSumAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(Sum<X, A> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, SumTransducer<Unit, Unit, Unit, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, SumTransducer<Unit, Unit, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducer<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducer<Unit, Unit, A, B>>, _) => TResult.Continue(s),
                (_, SumLeft<X, A>) => TResult.Continue(s),
                
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, SumTransducer<Unit, Unit, Unit, B>>> ToAsync() =>
            new FlattenSumTransducerAsyncSync3<X, Y, A, B>.Reduce1<S>(Value, Reducer.ToAsync());
    }
    
    record Reduce2<S>(A Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Unit, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Unit, B>> ToAsync() =>
            new FlattenSumTransducerAsyncSync3<X, Y, A, B>.Reduce2<S>(Value, Reducer.ToAsync());
    }    
}

record FlattenSumTransducer4<X, Y, A, B>(SumTransducer<X, Y, A, SumTransducer<Unit, Y, Unit, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        new FlattenSumTransducerAsyncSync4<X, Y, A, B>(FF.ToSumAsync());
    
    record Reduce<S>(SumTransducer<X, Y, A, SumTransducer<Unit, Y, Unit, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            new FlattenSumTransducerAsyncSync4<X, Y, A, B>.Reduce<S>(FF.ToSumAsync(), Reducer.ToAsync());
    }    
    
    record Reduce1<S>(Sum<X, A> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, SumTransducer<Unit, Y, Unit, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, SumTransducer<Unit, Y, Unit, B>> sf) =>
            (sf, Value) switch
            {
                (SumRight<Y, SumTransducer<Unit, Unit, A, B>> f, SumRight<X, A> r) => 
                    f.Value.Transform(new Reduce2<S>(r.Value, Reducer)).Run(st, s, Sum<Unit, A>.Right(r.Value)),
                        
                (SumLeft<Y, SumTransducer<Unit, Unit, A, B>>, _) => TResult.Continue(s),
                (_, SumLeft<X, A>) => TResult.Continue(s),
                
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, SumTransducer<Unit, Y, Unit, B>>> ToAsync() =>
            new FlattenSumTransducerAsyncSync4<X, Y, A, B>.Reduce1<S>(Value, Reducer.ToAsync());
    }
    
    record Reduce2<S>(A Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Unit, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Unit, B> ub) =>
            ub switch
            {
                SumRight<Unit, B> r => Reducer.Run(st, s, Sum<Y, B>.Right(r.Value)),
                SumLeft<Unit, B> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Unit, B>> ToAsync() =>
            new FlattenSumTransducerAsyncSync4<X, Y, A, B>.Reduce2<S>(Value, Reducer.ToAsync());
    }    
}

record FlattenSumTransducer5<X, Y, A, B>(Transducer<A, SumTransducer<X, Y, A, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        throw new NotImplementedException();
    
    record Reduce<S>(Transducer<A, SumTransducer<X, Y, A, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            x switch
            {
                SumRight<X, A> r => FF.Transform(new Reduce1<S>(x, Reducer)).Run(st, s, r.Value),
                SumLeft<X, A> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            throw new NotImplementedException();
    }    
    
    record Reduce1<S>(Sum<X, A> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, SumTransducer<X, Y, A, B>>
    {
        public override TResult<S> Run(TState st, S s, SumTransducer<X, Y, A, B> f) =>
            f.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, SumTransducer<X, Y, A, B>> ToAsync() =>
            throw new NotImplementedException();
    }
}

record FlattenSumTransducer6<X, Y, A, B>(Transducer<A, SumTransducer<X, Y, Unit, B>> FF) : SumTransducer<X, Y, A, B>
{
    static readonly Sum<X, Unit> runit = Sum<X, Unit>.Right(default);
    
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);
    
    record Reduce<S>(Transducer<A, SumTransducer<X, Y, Unit, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            x switch
            {
                SumRight<X, A> r => FF.Transform(new Reduce1<S>(runit, Reducer)).Run(st, s, r.Value),
                SumLeft<X, A> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            throw new NotImplementedException();
    }    
    
    record Reduce1<S>(Sum<X, Unit> Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, SumTransducer<X, Y, Unit, B>>
    {
        public override TResult<S> Run(TState st, S s, SumTransducer<X, Y, Unit, B> f) =>
            f.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, SumTransducer<X, Y, Unit, B>> ToAsync() =>
            throw new NotImplementedException();
    }

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        throw new NotImplementedException();
}

record FlattenSumTransducer7<X, Y, A, B>(SumTransducer<X, Y, A, Transducer<A, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        throw new NotImplementedException();
    
    record Reduce<S>(SumTransducer<X, Y, A, Transducer<A, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            x switch
            {
                SumRight<X, A> r => FF.Transform(new Reduce1<S>(r.Value, Reducer)).Run(st, s, x),
                SumLeft<X, A> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            throw new NotImplementedException();
    }    
    
    record Reduce1<S>(A Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, Transducer<A, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, Transducer<A, B>> sf) =>
            sf switch
            {
                SumRight<Y, Transducer<A, B>> f => f.Value.Transform(new Reduce2<S>(Reducer)).Run(st, s, Value),
                SumLeft<Y, Transducer<A, B>> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, Transducer<A, B>>> ToAsync() =>
            throw new NotImplementedException();
    }

    record Reduce2<S>(Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Sum<Y, B>.Right(value), Reducer);

        public override ReducerAsync<S, B> ToAsync() =>
            throw new NotImplementedException();
    }
}

record FlattenSumTransducer8<X, Y, A, B>(SumTransducer<X, Y, A, Transducer<Unit, B>> FF) : SumTransducer<X, Y, A, B>
{
    public override Reducer<S, Sum<X, A>> Transform<S>(Reducer<S, Sum<Y, B>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override SumTransducerAsync<X, Y, A, B> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<X, Y, A, B> ToSumAsync() =>
        throw new NotImplementedException();
    
    record Reduce<S>(SumTransducer<X, Y, A, Transducer<Unit, B>> FF, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<X, A>>
    {
        public override TResult<S> Run(TState st, S s, Sum<X, A> x) =>
            x switch
            {
                SumRight<X, A> r => FF.Transform(new Reduce1<S>(r.Value, Reducer)).Run(st, s, x),
                SumLeft<X, A> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<X, A>> ToAsync() =>
            throw new NotImplementedException();
    }    
    
    record Reduce1<S>(A Value, Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, Sum<Y, Transducer<Unit, B>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, Transducer<Unit, B>> sf) =>
            sf switch
            {
                SumRight<Y, Transducer<Unit, B>> f => f.Value.Transform(new Reduce2<S>(Reducer)).Run(st, s, default),
                SumLeft<Y, Transducer<Unit, B>> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, Transducer<Unit, B>>> ToAsync() =>
            throw new NotImplementedException();
    }

    record Reduce2<S>(Reducer<S, Sum<Y, B>> Reducer) : Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B value) =>
            TResult.Recursive(st, s, Sum<Y, B>.Right(value), Reducer);

        public override ReducerAsync<S, B> ToAsync() =>
            throw new NotImplementedException();
    }
}
