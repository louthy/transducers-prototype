#nullable enable
namespace LanguageExt;

record ComposeTransducer<TA, TB, TC>(
    Transducer<TA, TB> F, 
    Transducer<TB, TC> G) : 
    Transducer<TA, TC>
{
    public override Reducer<S, TA> Transform<S>(Reducer<S, TC> reduce) =>
        F.Transform(
            G.Transform(reduce));

    public override TransducerAsync<TA, TC> ToAsync() =>
        new ComposeTransducerAsync<TA, TB, TC>(F.ToAsync(), G.ToAsync());
}

record ComposeTransducer<TA, TB, TC, TD>(
    Transducer<TA, TB> F, 
    Transducer<TB, TC> G, 
    Transducer<TC, TD> H) : 
    Transducer<TA, TD>
{
    public override Reducer<S, TA> Transform<S>(Reducer<S, TD> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(reduce)));

    public override TransducerAsync<TA, TD> ToAsync() =>
        new ComposeTransducerAsync<TA, TB, TC, TD>(F.ToAsync(), G.ToAsync(), H.ToAsync());
}

record ComposeTransducer<TA, TB, TC, TD, TE>(
    Transducer<TA, TB> F, 
    Transducer<TB, TC> G, 
    Transducer<TC, TD> H, 
    Transducer<TD, TE> I) : 
    Transducer<TA, TE>
{
    public override Reducer<S, TA> Transform<S>(Reducer<S, TE> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(reduce))));

    public override TransducerAsync<TA, TE> ToAsync() =>
        new ComposeTransducerAsync<TA, TB, TC, TD, TE>(F.ToAsync(), G.ToAsync(), H.ToAsync(), I.ToAsync());
}

record ComposeTransducer<TA, TB, TC, TD, TE, TF>(
    Transducer<TA, TB> F, 
    Transducer<TB, TC> G, 
    Transducer<TC, TD> H, 
    Transducer<TD, TE> I, 
    Transducer<TE, TF> J) : 
    Transducer<TA, TF>
{
    public override Reducer<S, TA> Transform<S>(Reducer<S, TF> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(reduce)))));

    public override TransducerAsync<TA, TF> ToAsync() =>
        new ComposeTransducerAsync<TA, TB, TC, TD, TE, TF>(
            F.ToAsync(), G.ToAsync(), H.ToAsync(), I.ToAsync(), J.ToAsync());
}

record ComposeTransducer<TA, TB, TC, TD, TE, TF, TG>(
    Transducer<TA, TB> F, 
    Transducer<TB, TC> G, 
    Transducer<TC, TD> H, 
    Transducer<TD, TE> I, 
    Transducer<TE, TF> J, 
    Transducer<TF, TG> K) : 
    Transducer<TA, TG>
{
    public override Reducer<S, TA> Transform<S>(Reducer<S, TG> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(
                            K.Transform(reduce))))));

    public override TransducerAsync<TA, TG> ToAsync() =>
        new ComposeTransducerAsync<TA, TB, TC, TD, TE, TF, TG>(
            F.ToAsync(), G.ToAsync(), H.ToAsync(), I.ToAsync(), J.ToAsync(), K.ToAsync());
}