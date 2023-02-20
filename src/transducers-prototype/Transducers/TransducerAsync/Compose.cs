#nullable enable
namespace LanguageExt;

record ComposeTransducerAsync<TA, TB, TC>(
    TransducerAsync<TA, TB> F, 
    TransducerAsync<TB, TC> G) : 
    TransducerAsync<TA, TC>
{
    public override ReducerAsync<S, TA> Transform<S>(ReducerAsync<S, TC> reduce) =>
        F.Transform(
            G.Transform(reduce));
}

record ComposeTransducerAsync<TA, TB, TC, TD>(
    TransducerAsync<TA, TB> F, 
    TransducerAsync<TB, TC> G, 
    TransducerAsync<TC, TD> H) : 
    TransducerAsync<TA, TD>
{
    public override ReducerAsync<S, TA> Transform<S>(ReducerAsync<S, TD> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(reduce)));
}

record ComposeTransducerAsync<TA, TB, TC, TD, TE>(
    TransducerAsync<TA, TB> F, 
    TransducerAsync<TB, TC> G, 
    TransducerAsync<TC, TD> H, 
    TransducerAsync<TD, TE> I) : 
    TransducerAsync<TA, TE>
{
    public override ReducerAsync<S, TA> Transform<S>(ReducerAsync<S, TE> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(reduce))));
}

record ComposeTransducerAsync<TA, TB, TC, TD, TE, TF>(
    TransducerAsync<TA, TB> F, 
    TransducerAsync<TB, TC> G, 
    TransducerAsync<TC, TD> H, 
    TransducerAsync<TD, TE> I, 
    TransducerAsync<TE, TF> J) : 
    TransducerAsync<TA, TF>
{
    public override ReducerAsync<S, TA> Transform<S>(ReducerAsync<S, TF> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(reduce)))));
}

record ComposeTransducerAsync<TA, TB, TC, TD, TE, TF, TG>(
    TransducerAsync<TA, TB> F, 
    TransducerAsync<TB, TC> G, 
    TransducerAsync<TC, TD> H, 
    TransducerAsync<TD, TE> I, 
    TransducerAsync<TE, TF> J, 
    TransducerAsync<TF, TG> K) : 
    TransducerAsync<TA, TG>
{
    public override ReducerAsync<S, TA> Transform<S>(ReducerAsync<S, TG> reduce) =>
        F.Transform(
            G.Transform(
                H.Transform(
                    I.Transform(
                        J.Transform(
                            K.Transform(reduce))))));
}