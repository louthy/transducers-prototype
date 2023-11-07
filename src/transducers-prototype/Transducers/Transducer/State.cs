/*
#nullable enable
namespace LanguageExt;

record StateTransducer<State, A>(Transducer<State, (State State, A Value)> MorphismValue) :
    Transducer<State, A>
{
    public override Reducer<S, State> Transform<S>(Reducer<S, A> reduce) =>
        MorphismValue.Transform(new Reducer1<S>(reduce));

    record Reducer1<S>(Reducer<S, A> Reducer) : Reducer<S, (State State, A Value)>
    {
        //                                             THIS IS A PROBLEM
        public override TResult<S> Run(TState st, S s, (State State, A Value) value) =>
            Reducer.Run(st, s, value.Value);
    }
}

record StateSumTransducer<Env, A>(SumTransducer<Env, ValueTask<Error>, Env, ValueTask<A>> MorphismValue) :
    SumTransducer<Env, Error, Env, A>
{
    public override Reducer<S, Sum<Env, Env>> Transform<S>(Reducer<S, Sum<Error, A>> reduce) =>
        MorphismValue.Transform(new Reducer1<S>(reduce));

    record Reducer1<S>(Reducer<S, Sum<Error, A>> Reducer) : Reducer<S, Sum<ValueTask<Error>, ValueTask<A>>>
    {
        public override TResult<S> Run(TState st, S s, Sum<ValueTask<Error>, ValueTask<A>> value)
        {
            var result = TResult.Complete(s);
            using var wait = new ManualResetEventSlim(false);
            Go(value, wait);
            wait.Wait();
            return result;

            async ValueTask Go(Sum<ValueTask<Error>, ValueTask<A>> t, ManualResetEventSlim handle)
            {
                try
                {
                    switch (t)
                    {
                        case SumRight<ValueTask<Error>, ValueTask<A>> tr:
                            result = Reducer.Run(st, s, Sum<Error, A>.Right(await tr.Value.ConfigureAwait(false)));
                            handle.Set();
                            break;

                        case SumLeft<ValueTask<Error>, ValueTask<A>> tl:
                            result = Reducer.Run(st, s, Sum<Error, A>.Left(await tl.Value.ConfigureAwait(false)));
                            handle.Set();
                            break;

                        default:
                            return;
                    }
                }
                catch (Exception e)
                {
                    result = Reducer.Run(st, s, Sum<Error, A>.Left(e));
                    handle.Set();
                }
            }
        }
    }
}
*/
