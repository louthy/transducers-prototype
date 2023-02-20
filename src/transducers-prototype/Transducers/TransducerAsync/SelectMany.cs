#nullable enable
namespace LanguageExt;

record SelectManyTransducerAsync2<A, B, C, D>(TransducerAsync<A, B> F, Func<B, TransducerAsync<A, C>> BindF, Func<B, C, D> Project) : 
    TransducerAsync<A, D>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, D> reduce) =>
        new Reduce<S>(F, BindF, Project, reduce);

    internal record Reduce<S>(TransducerAsync<A, B> F, Func<B, TransducerAsync<A, C>> Bind, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) : 
        ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            F.Transform(new Binder<S>(x, Bind, Project, Reducer)).Run(st, s, x);
    }

    internal record Binder<S>(A Value, Func<B, TransducerAsync<A, C>> Bind, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B b) =>
            Bind(b).Transform(new Projector<S>(b, Project, Reducer)).Run(st, s, Value);
    }
    
    internal record Projector<S>(B Value, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, C>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, C c) =>
            Reducer.Run(st, s, Project(Value, c));
    }
}

record SelectManyTransducerAsyncSync3<A, B, C, D>(
    TransducerAsync<A, B> F, 
    TransducerAsync<B, Transducer<A, C>> BindF, 
    Func<B, C, D> Project) : 
    TransducerAsync<A, D>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, D> reduce) =>
        new Reduce<S>(F, BindF, Project, reduce);

    internal record Reduce<S>(
        TransducerAsync<A, B> F, 
        TransducerAsync<B, Transducer<A, C>> Bind, 
        Func<B, C, D> Project, ReducerAsync<S, D> Reducer) : 
        ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            F.Transform(new Binder<S>(x, Bind, Project, Reducer)).Run(st, s, x);
    }

    internal record Binder<S>(A Value, TransducerAsync<B, Transducer<A, C>> Bind, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B b) =>
            Bind.Transform(new BindApply<S>(Value, b, Project, Reducer)).Run(st, s, b);
    }

    internal record BindApply<S>(A ValueX, B ValueY, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) : ReducerAsync<S, Transducer<A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, Transducer<A, C> t) =>
            TResultAsync.Recursive(st, s, ValueX, t.ToAsync().Transform(new Projector<S>(ValueY, Project, Reducer)));
    }

    internal record Projector<S>(B Value, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, C>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, C c) =>
            Reducer.Run(st, s, Project(Value, c));
    }
}

record SelectManyTransducerAsync3<A, B, C, D>(
    TransducerAsync<A, B> F, 
    TransducerAsync<B, TransducerAsync<A, C>> BindF, 
    Func<B, C, D> Project) : 
    TransducerAsync<A, D>
{
    public override ReducerAsync<S, A> Transform<S>(ReducerAsync<S, D> reduce) =>
        new Reduce<S>(F, BindF, Project, reduce);

    record Reduce<S>(
        TransducerAsync<A, B> F, 
        TransducerAsync<B, TransducerAsync<A, C>> Bind, 
        Func<B, C, D> Project, ReducerAsync<S, D> Reducer) : 
        ReducerAsync<S, A>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, A x) =>
            F.Transform(new Binder<S>(x, Bind, Project, Reducer)).Run(st, s, x);
    }

    record Binder<S>(A Value, TransducerAsync<B, TransducerAsync<A, C>> Bind, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, B>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, B b) =>
            Bind.Transform(new BindApply<S>(Value, b, Project, Reducer)).Run(st, s, b);
    }

    record BindApply<S>(A ValueX, B ValueY, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) : ReducerAsync<S, TransducerAsync<A, C>>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, TransducerAsync<A, C> t) =>
            new(TResultAsync.Recursive(st, s, ValueX, t.Transform(new Projector<S>(ValueY, Project, Reducer))));
    }

    record Projector<S>(B Value, Func<B, C, D> Project, ReducerAsync<S, D> Reducer) :
        ReducerAsync<S, C>
    {
        public override ValueTask<TResultAsync<S>> Run(TState st, S s, C c) =>
            Reducer.Run(st, s, Project(Value, c));
    }
}
