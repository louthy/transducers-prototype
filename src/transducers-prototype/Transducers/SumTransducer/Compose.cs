#nullable enable
namespace LanguageExt;

record ComposeSumTransducer<TT, TU, TV, TA, TB, TC>(
    SumTransducer<TT, TU, TA, TB> F, 
    SumTransducer<TU, TV, TB, TC> G) : 
    SumTransducer<TT, TV, TA, TC>
{
    public override Reducer<S, Sum<TT, TA>> Transform<S>(Reducer<S, Sum<TV, TC>> reduce) =>
        F.Transform(
            G.Transform(reduce));

    public override SumTransducerAsync<TT, TV, TA, TC> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<TT, TV, TA, TC> ToSumAsync() =>
        new ComposeSumTransducerAsync<TT, TU, TV, TA, TB, TC>(F.ToSumAsync(), G.ToSumAsync());
}

record ComposeSumTransducer<TT, TU, TV, TW, TA, TB, TC, TD>(
    SumTransducer<TT, TU, TA, TB> F, 
    SumTransducer<TU, TV, TB, TC> G, 
    SumTransducer<TV, TW, TC, TD> H) : 
    SumTransducer<TT, TW, TA, TD>
{
    public override Reducer<S, Sum<TT, TA>> Transform<S>(Reducer<S, Sum<TW, TD>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(reduce)));

    public override SumTransducerAsync<TT, TW, TA, TD> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<TT, TW, TA, TD> ToSumAsync() =>
        new ComposeSumTransducerAsync<TT, TU, TV, TW, TA, TB, TC, TD>(F.ToSumAsync(), G.ToSumAsync(), H.ToSumAsync());
}

record ComposeSumTransducer<TT, TU, TV, TW, TX, TA, TB, TC, TD, TE>(
    SumTransducer<TT, TU, TA, TB> F, 
    SumTransducer<TU, TV, TB, TC> G, 
    SumTransducer<TV, TW, TC, TD> H, 
    SumTransducer<TW, TX, TD, TE> I) : 
    SumTransducer<TT, TX, TA, TE>
{
    public override Reducer<S, Sum<TT, TA>> Transform<S>(Reducer<S, Sum<TX, TE>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(reduce))));

    public override SumTransducerAsync<TT, TX, TA, TE> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<TT, TX, TA, TE> ToSumAsync() =>
        new ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TA, TB, TC, TD, TE>(
            F.ToSumAsync(), G.ToSumAsync(), H.ToSumAsync(), I.ToSumAsync());
}

record ComposeSumTransducer<TT, TU, TV, TW, TX, TY, TA, TB, TC, TD, TE, TF>(
    SumTransducer<TT, TU, TA, TB> F, 
    SumTransducer<TU, TV, TB, TC> G, 
    SumTransducer<TV, TW, TC, TD> H, 
    SumTransducer<TW, TX, TD, TE> I, 
    SumTransducer<TX, TY, TE, TF> J) : 
    SumTransducer<TT, TY, TA, TF>
{
    public override Reducer<S, Sum<TT, TA>> Transform<S>(Reducer<S, Sum<TY, TF>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(reduce)))));

    public override SumTransducerAsync<TT, TY, TA, TF> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<TT, TY, TA, TF> ToSumAsync() =>
        new ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TY, TA, TB, TC, TD, TE, TF>(
            F.ToSumAsync(), G.ToSumAsync(), H.ToSumAsync(), I.ToSumAsync(), J.ToSumAsync());
}

record ComposeSumTransducer<TT, TU, TV, TW, TX, TY, TZ, TA, TB, TC, TD, TE, TF, TG>(
    SumTransducer<TT, TU, TA, TB> F, 
    SumTransducer<TU, TV, TB, TC> G, 
    SumTransducer<TV, TW, TC, TD> H, 
    SumTransducer<TW, TX, TD, TE> I, 
    SumTransducer<TX, TY, TE, TF> J, 
    SumTransducer<TY, TZ, TF, TG> K) : 
    SumTransducer<TT, TZ, TA, TG>
{
    public override Reducer<S, Sum<TT, TA>> Transform<S>(Reducer<S, Sum<TZ, TG>> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(
                            K.Transform(reduce))))));

    public override SumTransducerAsync<TT, TZ, TA, TG> ToAsync() =>
        ToSumAsync();

    public override SumTransducerAsync<TT, TZ, TA, TG> ToSumAsync() =>
        new ComposeSumTransducerAsync<TT, TU, TV, TW, TX, TY, TZ, TA, TB, TC, TD, TE, TF, TG>(
            F.ToSumAsync(), G.ToSumAsync(), H.ToSumAsync(), I.ToSumAsync(), J.ToSumAsync(), K.ToSumAsync());
}
