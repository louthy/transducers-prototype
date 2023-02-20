#nullable enable
namespace LanguageExt;

record SelectManyTransducer1<A, B, C>(Transducer<A, B> F, Func<B, Transducer<A, C>> BindF) : 
    Transducer<A, C>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, C> reduce) =>
        new Reduce<S>(F, BindF, reduce);

    public override TransducerAsync<A, C> ToAsync() =>
        new SelectManyTransducerAsync1<A, B, C>(F.ToAsync(), x => BindF(x).ToAsync());

    record Reduce<S>(Transducer<A, B> F, Func<B, Transducer<A, C>> Bind, Reducer<S, C> Reducer) : 
        Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            TResult.Recursive(st, s, x, F.Transform(new Binder<S>(x, Bind, Reducer)));

        public override ReducerAsync<S, A> ToAsync() =>
            new SelectManyTransducerAsync1<A, B, C>.Reduce<S>(F.ToAsync(), x => Bind(x).ToAsync(), Reducer.ToAsync());
    }

    record Binder<S>(A Value, Func<B, Transducer<A, C>> Bind, Reducer<S, C> Reducer) :
        Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B b) =>
            Bind(b).Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, B> ToAsync() =>
            new SelectManyTransducerAsync1<A, B, C>.Binder<S>(Value, x => Bind(x).ToAsync(), Reducer.ToAsync());
    }
}

record SelectManyTransducer2<A, B, C, D>(
    Transducer<A, B> F, 
    Func<B, Transducer<A, C>> BindF, 
    Func<B, C, D> Project) : 
    Transducer<A, D>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, D> reduce) =>
        new Reduce<S>(F, BindF, Project, reduce);

    public override TransducerAsync<A, D> ToAsync() =>
        new SelectManyTransducerAsync2<A, B, C, D>(F.ToAsync(), x => BindF(x).ToAsync(), Project);

    record Reduce<S>(Transducer<A, B> F, Func<B, Transducer<A, C>> Bind, Func<B, C, D> Project, Reducer<S, D> Reducer) : 
        Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            F.Transform(new Binder<S>(x, Bind, Project, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, A> ToAsync() =>
            new SelectManyTransducerAsync2<A, B, C, D>.Reduce<S>(
                F.ToAsync(), x => Bind(x).ToAsync(), Project, Reducer.ToAsync());
    }

    record Binder<S>(A Value, Func<B, Transducer<A, C>> Bind, Func<B, C, D> Project, Reducer<S, D> Reducer) :
        Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B b) =>
            TResult.Recursive(st, s, Value, Bind(b).Transform(new Projector<S>(b, Project, Reducer)));

        public override ReducerAsync<S, B> ToAsync() =>
            new SelectManyTransducerAsync2<A, B, C, D>.Binder<S>(
                Value, x => Bind(x).ToAsync(), Project, Reducer.ToAsync());
    }
    
    record Projector<S>(B Value, Func<B, C, D> Project, Reducer<S, D> Reducer) :
        Reducer<S, C>
    {
        public override TResult<S> Run(TState st, S s, C c) =>
          Reducer.Run(st, s, Project(Value, c));

        public override ReducerAsync<S, C> ToAsync() =>
            new SelectManyTransducerAsync2<A, B, C, D>.Projector<S>(Value, Project, Reducer.ToAsync());
    }
}

record SelectManyTransducer3<A, B, C, D>(
    Transducer<A, B> F, 
    Transducer<B, Transducer<A, C>> BindF, 
    Func<B, C, D> Project) : 
    Transducer<A, D>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, D> reduce) =>
        new Reduce<S>(F, BindF, Project, reduce);

    public override TransducerAsync<A, D> ToAsync() =>
        new SelectManyTransducerAsyncSync3<A, B, C, D>(F.ToAsync(), BindF.ToAsync(), Project);

    record Reduce<S>(Transducer<A, B> F, Transducer<B, Transducer<A, C>> Bind, Func<B, C, D> Project, Reducer<S, D> Reducer) : 
        Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A x) =>
            F.Transform(new Binder<S>(x, Bind, Project, Reducer)).Run(st, s, x);

        public override ReducerAsync<S, A> ToAsync() =>
            new SelectManyTransducerAsyncSync3<A, B, C, D>.Reduce<S>(F.ToAsync(), Bind.ToAsync(), Project, Reducer.ToAsync());
    }

    record Binder<S>(A Value, Transducer<B, Transducer<A, C>> Bind, Func<B, C, D> Project, Reducer<S, D> Reducer) :
        Reducer<S, B>
    {
        public override TResult<S> Run(TState st, S s, B b) =>
            Bind.Transform(new BindApply<S>(Value, b, Project, Reducer)).Run(st, s, b);

        public override ReducerAsync<S, B> ToAsync() =>
            new SelectManyTransducerAsyncSync3<A, B, C, D>.Binder<S>(Value, Bind.ToAsync(), Project, Reducer.ToAsync());
    }

    record BindApply<S>(A ValueX, B ValueY, Func<B, C, D> Project, Reducer<S, D> Reducer) : Reducer<S, Transducer<A, C>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<A, C> t) =>
            TResult.Recursive(st, s, ValueX, t.Transform(new Projector<S>(ValueY, Project, Reducer)));

        public override ReducerAsync<S, Transducer<A, C>> ToAsync() =>
            new SelectManyTransducerAsyncSync3<A, B, C, D>.BindApply<S>(ValueX, ValueY, Project, Reducer.ToAsync());
    }

    record Projector<S>(B Value, Func<B, C, D> Project, Reducer<S, D> Reducer) :
        Reducer<S, C>
    {
        public override TResult<S> Run(TState st, S s, C c) =>
            Reducer.Run(st, s, Project(Value, c));

        public override ReducerAsync<S, C> ToAsync() =>
            new SelectManyTransducerAsyncSync3<A, B, C, D>.Projector<S>(Value, Project, Reducer.ToAsync());
    }
}
