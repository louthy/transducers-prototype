namespace LanguageExt;

record App<Env, A, B>(Transducer<Env, Transducer<A, B>> F, Env Value) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        new Reduce1<S>(F, Value, reduce);

    public override TransducerAsync<A, B> ToAsync() =>
        throw new NotImplementedException();

    record Reduce1<S>(Transducer<Env, Transducer<A, B>> F, Env Value, Reducer<S, B> Reducer) : Reducer<S, A>
    {
        public override TResult<S> Run(TState st, S s, A value) =>
            F.Transform(new Reduce2<S>(value, Reducer)).Run(st, s, Value);

        public override ReducerAsync<S, A> ToAsync() =>
            throw new NotImplementedException();
    }

    record Reduce2<S>(A Value, Reducer<S, B> Reducer) : Reducer<S, Transducer<A, B>>
    {
        public override TResult<S> Run(TState st, S s, Transducer<A, B> t) =>
            t.Transform(Reducer).Run(st, s, Value);

        public override ReducerAsync<S, Transducer<A, B>> ToAsync() =>
            throw new NotImplementedException();
    }
}