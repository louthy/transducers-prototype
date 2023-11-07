namespace LanguageExt.Examples;

public static class Application<Env>
{
    public static Application<Env, A> Success<A>(A value) =>
        new (EitherT<MReaderT<MIO<Env>, Env>, Env, Error, A>.Right(value));
    
    public static Application<Env, A> Fail<A>(Error value) =>
        new (EitherT<MReaderT<MIO<Env>, Env>, Env, Error, A>.Left(value));

    public static Application<Env, A> Lift<A>(ReaderT<MIO<Env>, Env, A> value) =>
        new (EitherT<MReaderT<MIO<Env>, Env>, Env, Error, A>.Lift(value.Morphism));
    
    public static readonly Application<Env, Env> Ask =
        Lift(ReaderT<MIO<Env>, Env>.Ask);
}

public record Application<Env, A>(EitherT<MReaderT<MIO<Env>, Env>, Env, Error, A> Morphism) 
{
    public static Application<Env, A> Pure(A value) =>
        new (EitherT<MReaderT<MIO<Env>, Env>, Env, Error, A>.Right(value));

    public Application<Env, B> Map<B>(Func<A, B> f) =>
        new (Morphism.MapRight(f));

    public Application<Env, B> Select<B>(Func<A, B> f) =>
        new (Morphism.MapRight(f));

    public Application<Env, B> Bind<B>(Func<A, Application<Env, B>> f) =>
        new (Morphism.Bind(x => f(x).Morphism));

    public Application<Env, C> SelectMany<B, C>(Func<A, Application<Env, B>> bind, Func<A, B, C> f) =>
        Bind(x => bind(x).Map(y => f(x, y)));

    public Application<Env, B> BiMap<B>(Func<Error, Error> Fail, Func<A, B> Succ, Func<B>? Bottom = null) =>
        new(Morphism.BiMap(Fail, Succ));
}
