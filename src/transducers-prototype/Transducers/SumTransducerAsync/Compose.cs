#nullable enable
namespace LanguageExt;

record ComposeSumTransducerAsync<TT, TU, TV, TA, TB, TC>(
    SumTransducerAsync<TT, TU, TA, TB> F, 
    SumTransducerAsync<TU, TV, TB, TC> G) : 
    SumTransducerAsync<TT, TV, TA, TC>
{
    public override ReducerAsync<S, Sum<TT, TA>> Transform<S>(ReducerAsync<S, Sum<TV, TC>> reduce) =>
        F.Transform(
            G.Transform(reduce));
}

record ComposeSumTransducerAsync<TT, TU, TV, TW, TA, TB, TC, TD>(
    SumTransducerAsync<TT, TU, TA, TB> F, 
    SumTransducerAsync<TU, TV, TB, TC> G, 
    SumTransducerAsync<TV, TW, TC, TD> H) : 
    SumTransducerAsync<TT, TW, TA, TD>
{
    public override ReducerAsync<S, Sum<TT, TA>> Transform<S>(ReducerAsync<S, Sum<TW, TD>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(reduce)));
}

record ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TA, TB, TC, TD, TE>(
    SumTransducerAsync<TT, TU, TA, TB> F, 
    SumTransducerAsync<TU, TV, TB, TC> G, 
    SumTransducerAsync<TV, TW, TC, TD> H, 
    SumTransducerAsync<TW, TX, TD, TE> I) : 
    SumTransducerAsync<TT, TX, TA, TE>
{
    public override ReducerAsync<S, Sum<TT, TA>> Transform<S>(ReducerAsync<S, Sum<TX, TE>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(reduce))));
}

record ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TY, TA, TB, TC, TD, TE, TF>(
    SumTransducerAsync<TT, TU, TA, TB> F, 
    SumTransducerAsync<TU, TV, TB, TC> G, 
    SumTransducerAsync<TV, TW, TC, TD> H, 
    SumTransducerAsync<TW, TX, TD, TE> I, 
    SumTransducerAsync<TX, TY, TE, TF> J) : 
    SumTransducerAsync<TT, TY, TA, TF>
{
    public override ReducerAsync<S, Sum<TT, TA>> Transform<S>(ReducerAsync<S, Sum<TY, TF>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(reduce)))));
}

record ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TY, TZ, TA, TB, TC, TD, TE, TF, TG>(
    SumTransducerAsync<TT, TU, TA, TB> F, 
    SumTransducerAsync<TU, TV, TB, TC> G, 
    SumTransducerAsync<TV, TW, TC, TD> H, 
    SumTransducerAsync<TW, TX, TD, TE> I, 
    SumTransducerAsync<TX, TY, TE, TF> J, 
    SumTransducerAsync<TY, TZ, TF, TG> K) : 
    SumTransducerAsync<TT, TZ, TA, TG>
{
    public override ReducerAsync<S, Sum<TT, TA>> Transform<S>(ReducerAsync<S, Sum<TZ, TG>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(
                            K.Transform(reduce))))));
}
