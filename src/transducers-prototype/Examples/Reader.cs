namespace LanguageExt.Examples;

/*
public record Reader<Env>
{
    public static Reader<Env, A> Pure<A>(A value) => 
        new (ReaderT<MIdentity<Env>, Env>.Pure(value));

    public static readonly Reader<Env, Env> Ask =
        new(ReaderT<MIdentity<Env>, Env>.Ask);
}
*/

public record Reader<Env, A>(Transducer<Env, A> Morphism)
{
    public Reader<Env, B> Map<B>(Func<A, B> f) =>
        new (Morphism.Map(f));

    public Reader<Env, B> Select<B>(Func<A, B> f) =>
        new (Morphism.Map(f));

    public Reader<Env, B> Bind<B>(Func<A, Reader<Env, B>> f) =>
        new (Morphism.Bind(x => f(x).Morphism));

    public Reader<Env, C> SelectMany<B, C>(Func<A, Reader<Env, B>> bind, Func<A, B, C> f) =>
        Bind(x => bind(x).Map(y => f(x, y)));
}

public static class Reader
{
    public static Reader<Env, A> Flatten<Env, A>(this Reader<Env, Reader<Env, A>> mma) =>
        new(mma.Morphism.Map(static m => m.Morphism).Flatten());

    public static Transducer<Transducer<Env, A>, Transducer<Env, B>> bind<Env, A, B>(Transducer<A, Transducer<Env, B>> f) =>
        Transducer.lift<Transducer<Env, A>, Transducer<Env, B>>(ra =>
            Transducer.compose(ra.Morphism, f).Flatten());
}