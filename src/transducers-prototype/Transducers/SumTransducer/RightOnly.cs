/*namespace LanguageExt;

public record RightOnly<X, Y, A, B>(Transducer<Sum<X, A>, Sum<Y, B>> F) : Transducer<A, B>
{
    public override Reducer<S, A> Transform<S>(Reducer<S, B> reduce) =>
        F.Transform(new Reduce<S>(reduce));
    
    public override TransducerAsync<A, B> ToAsync()
    {
        throw new NotImplementedException();
    }

    record Reduce<S>(Reducer<S, B> Reducer) : Reducer<S, Sum<Y, B>>
    {
        public override TResult<S> Run(TState st, S s, Sum<Y, B> value) =>
            value switch
            {
                SumRight<Y, B> r => Reducer.Run(st, s, r.Value),
                SumLeft<Y, B> => TResult.Continue(s),
                _ => TResult.Complete(s)
            };

        public override ReducerAsync<S, Sum<Y, B>> ToAsync()
        {
            throw new NotImplementedException();
        }
    }
}*/