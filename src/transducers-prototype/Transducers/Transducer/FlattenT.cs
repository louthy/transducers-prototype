#nullable enable
using LanguageExt.HKT;

namespace LanguageExt;

public static partial class Transducer
{
    public static Transducer<B, C> App<A, B, C>(this Transducer<A, Transducer<B, C>> t, A value) =>
        new App<A, B, C>(t, value);
    
    public static Transducer<Env, Transducer<Env, A>> FlattenT<Env, A>(
        this Transducer<Env, Transducer<Env, Transducer<Env, Transducer<Env, A>>>> m) =>
        m.Flatten().Flatten();
    
    public static Transducer<Env, Transducer<Unit, A>> FlattenT<Env, A>(
        this Transducer<Env, Transducer<Unit, Transducer<Env, Transducer<Unit, A>>>> m) =>
        m.Flatten().Flatten();

    public static Transducer<Env, SumTransducer<Unit, X, Unit, A>> FlattenT<Env, X, A>(
        this Transducer<Env, SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>> m) =>
        new FlattenTTransducer1<Env, X, A>(m);

    public static Transducer<Env, SumTransducer<Env, X, Env, A>> FlattenT<Env, X, A>(
        this Transducer<Env, SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>>> m) =>
        new FlattenTTransducer2<Env, X, A>(m);
}

record FlattenTTransducer1<Env, X, A>(
    Transducer<Env, SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>> FF) : 
    Transducer<Env, SumTransducer<Unit, X, Unit, A>>
{
    public override Reducer<S, Env> Transform<S>(Reducer<S, SumTransducer<Unit, X, Unit, A>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override TransducerAsync<Env, SumTransducer<Unit, X, Unit, A>> ToAsync() =>
        throw new NotImplementedException();

    record Reduce<S>(
        Transducer<Env, SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>> FF,
        Reducer<S, SumTransducer<Unit, X, Unit, A>> Reducer) : 
        Reducer<S, Env>
    {
        public override TResult<S> Run(TState st, S s, Env value) =>
            FF.Transform(new Reduce1<S>(value, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, Env> ToAsync() =>
            throw new NotImplementedException();
    }
    
    record Reduce1<S>(Env Env, Reducer<S, SumTransducer<Unit, X, Unit, A>> Reducer) : 
        Reducer<S, SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>>
    {
        static readonly Sum<Unit, Unit> sunit = Sum<Unit, Unit>.Right(default);

        public override TResult<S> Run(
            TState st,
            S s,
            SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>> t) =>
            t.Transform(new Reduce2<S>(Env, Reducer)).Run(st, s, sunit);

        public override ReducerAsync<S, SumTransducer<Unit, X, Unit, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>> ToAsync()
        {
            throw new NotImplementedException();
        }
    }
    
    record Reduce2<S>(Env Env, Reducer<S, SumTransducer<Unit, X, Unit, A>> Reducer) : 
        Reducer<S, Sum<X, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>>
    {
        public override TResult<S> Run(
            TState st,
            S s,
            Sum<X, Transducer<Env, SumTransducer<Unit, X, Unit, A>>> t) =>
            t switch
            {
                SumRight<X, Transducer<Env, SumTransducer<Unit, X, Unit, A>>> r =>
                    r.Value.Transform(new Reduce3<S>(Reducer)).Run(st, s, Env),

                SumLeft<X, Transducer<Env, SumTransducer<Unit, X, Unit, A>>> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s),
            };

        public override ReducerAsync<S, Sum<X, Transducer<Env, SumTransducer<Unit, X, Unit, A>>>> ToAsync() =>
            throw new NotImplementedException();
    }
    
    record Reduce3<S>(Reducer<S, SumTransducer<Unit, X, Unit, A>> Reducer) : Reducer<S, SumTransducer<Unit, X, Unit, A>>
    {
        public override TResult<S> Run(
            TState st,
            S s,
            SumTransducer<Unit, X, Unit, A> t) =>
            Reducer.Run(st, s, t);

        public override ReducerAsync<S, SumTransducer<Unit, X, Unit, A>> ToAsync() =>
            throw new NotImplementedException();
    }
}

record FlattenTTransducer2<Env, X, A>(
    Transducer<Env, SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>>> FF) : 
    Transducer<Env, SumTransducer<Env, X, Env, A>>
{
    public override Reducer<S, Env> Transform<S>(Reducer<S, SumTransducer<Env, X, Env, A>> reduce) =>
        new Reduce<S>(FF, reduce);

    public override TransducerAsync<Env, SumTransducer<Env, X, Env, A>> ToAsync() =>
        throw new NotImplementedException();

    record Reduce<S>(
        Transducer<Env, SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>>> FF,
        Reducer<S, SumTransducer<Env, X, Env, A>> Reducer) : 
        Reducer<S, Env>
    {
        public override TResult<S> Run(TState st, S s, Env value) =>
            FF.Transform(new Reduce1<S>(value, Reducer)).Run(st, s, value);

        public override ReducerAsync<S, Env> ToAsync() =>
            throw new NotImplementedException();
    }
    
    record Reduce1<S>(Env Env, Reducer<S, SumTransducer<Env, X, Env, A>> Reducer) : 
        Reducer<S, SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>>>
    {
        public override TResult<S> Run(
            TState st,
            S s,
            SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>> t) =>
            t.Transform(new Reduce2<S>(Env, Reducer)).Run(st, s, Sum<Env, Env>.Right(Env));

        public override ReducerAsync<S, SumTransducer<Env, X, Env, Transducer<Env, SumTransducer<Env, X, Env, A>>>> ToAsync() =>
            throw new NotImplementedException();
    }
    
    record Reduce2<S>(Env Env, Reducer<S, SumTransducer<Env, X, Env, A>> Reducer) : 
        Reducer<S, Sum<X, Transducer<Env, SumTransducer<Env, X, Env, A>>>>
    {
        public override TResult<S> Run(
            TState st,
            S s,
            Sum<X, Transducer<Env, SumTransducer<Env, X, Env, A>>> t) =>
            t switch
            {
                SumRight<X, Transducer<Env, SumTransducer<Env, X, Env, A>>> r =>
                    r.Value.Transform(new Reduce3<S>(Reducer)).Run(st, s, Env),

                SumLeft<X, Transducer<Env, SumTransducer<Env, X, Env, A>>> =>
                    TResult.Continue(s),

                _ =>
                    TResult.Complete(s),
            };

        public override ReducerAsync<S, Sum<X, Transducer<Env, SumTransducer<Env, X, Env, A>>>> ToAsync() =>
            throw new NotImplementedException();
    }
    
    record Reduce3<S>(Reducer<S, SumTransducer<Env, X, Env, A>> Reducer) : Reducer<S, SumTransducer<Env, X, Env, A>>
    {
        public override TResult<S> Run(
            TState st,
            S s,
            SumTransducer<Env, X, Env, A> t) =>
            Reducer.Run(st, s, t);

        public override ReducerAsync<S, SumTransducer<Env, X, Env, A>> ToAsync() =>
            throw new NotImplementedException();
    }
}
