namespace LanguageExt;

record AsyncTransducer<Env, A>(Transducer<Env, ValueTask<A>> MorphismValue) :
    Transducer<Env, A>
{
    public override Reducer<S, Env> Transform<S>(Reducer<S, A> reduce) =>
        MorphismValue.Transform(new Reducer1<S>(reduce));

    public override TransducerAsync<Env, A> ToAsync()
    {
        throw new NotImplementedException();
    }

    record Reducer1<S>(Reducer<S, A> Reducer) : Reducer<S, ValueTask<A>>
    {
        public override TResult<S> Run(TState st, S s, ValueTask<A> value)
        {
            // Shortcut for tasks that are already completed, or never started because
            // they're just values.
            if (value.IsCompletedSuccessfully) return Reducer.Run(st, s, value.Result);
            
            var result = TResult.Complete(s);
            using var wait = new ManualResetEventSlim (false);
            #pragma warning disable CS4014
            Go(value, wait);
            #pragma warning restore CS4014
            wait.Wait();
            return result;

            async ValueTask Go(ValueTask<A> t, ManualResetEventSlim handle)
            {
                result = Reducer.Run(st, s, await t.ConfigureAwait(false));
                handle.Set();
            }
        }

        public override ReducerAsync<S, ValueTask<A>> ToAsync()
        {
            throw new NotImplementedException();
        }
    }
}


record AsyncSumTransducer<Env, A>(SumTransducer<Env, ValueTask<Error>, Env, ValueTask<A>> MorphismValue) :
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
            #pragma warning disable CS4014
            Go(value, wait);
            #pragma warning restore CS4014
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

        public override ReducerAsync<S, Sum<ValueTask<Error>, ValueTask<A>>> ToAsync()
        {
            throw new NotImplementedException();
        }
    }

    public override TransducerAsync<Sum<Env, Env>, Sum<Error, A>> ToAsync()
    {
        throw new NotImplementedException();
    }

    public override SumTransducerAsync<Env, Error, Env, A> ToSumAsync()
    {
        throw new NotImplementedException();
    }
}
