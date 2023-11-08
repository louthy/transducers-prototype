namespace LanguageExt.Examples;

public static class AppTest
{
    public static Application<string, string> Example1 =>
        from x in Application<string>.Success(100)
        from y in Application<string>.Success(200)
        from n in Application<string>.Ask
        select $"Hello {n}, the answer is: {x + y}";
    
    public static Application<string, string> Example2 =>
        from x in Application<string>.Success(100)
        from y in Application<string>.Fail<int>(Error.New("failed in example 2"))
        from n in Application<string>.Ask
        select $"Hello {n}, the answer is: {x + y}";
}

public static class Application<Env>
{
    public static Application<Env, A> Success<A>(A value) =>
        new (EitherT<MReaderT<MTry<Env>, Env>, Env, Error, A>.Right(value));
    
    public static Application<Env, A> Fail<A>(Error value) =>
        new (EitherT<MReaderT<MTry<Env>, Env>, Env, Error, A>.Left(value));

    public static Application<Env, A> Lift<A>(ReaderT<MTry<Env>, Env, A> value) =>
        new (EitherT<MReaderT<MTry<Env>, Env>, Env, Error, A>.Lift(value.Morphism));
    
    public static readonly Application<Env, Env> Ask =
        Lift(MReaderT<MTry<Env>, Env>.Ask);
}

public record Application<Env, A>(EitherT<MReaderT<MTry<Env>, Env>, Env, Error, A> Transformer) 
    : Transducer<Env, Sum<Error, A>>
{
    public Transducer<Env, Sum<Error, A>> Morphism =>
        Transformer.Morphism;
    
    public static Application<Env, A> Pure(A value) =>
        new (EitherT<MReaderT<MTry<Env>, Env>, Env, Error, A>.Right(value));

    public Application<Env, B> Map<B>(Func<A, B> f) =>
        new (Transformer.MapRight(f));

    public Application<Env, B> Select<B>(Func<A, B> f) =>
        new (Transformer.MapRight(f));

    public Application<Env, B> Bind<B>(Func<A, Application<Env, B>> f) =>
        new (Transformer.Bind(x => f(x).Transformer));

    public Application<Env, C> SelectMany<B, C>(Func<A, Application<Env, B>> bind, Func<A, B, C> f) =>
        Bind(x => bind(x).Map(y => f(x, y)));

    public Application<Env, B> BiMap<B>(Func<Error, Error> Fail, Func<A, B> Succ, Func<B>? Bottom = null) =>
        new(Transformer.BiMap(Fail, Succ));

    public Reducer<S, Env> Transform<S>(Reducer<S, Sum<Error, A>> reduce) =>
        Transformer.Morphism.Transform(reduce);
}
