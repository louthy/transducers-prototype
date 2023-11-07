/*using static LanguageExt.Transducer;

namespace LanguageExt.Examples;

public record Either<L, R>(EitherT<MIdentity, L, R> Morphism)
{
    public static Either<L, R> Right(R value) =>
        new (EitherT<MIdentity, L, R>.Right(value));
    
    public static Either<L, R> Left(L value) =>
        new (EitherT<MIdentity, L, R>.Left(value));

    public Either<L, B> MapRight<B>(Func<R, B> f) =>
        new (Morphism.MapRight(f));
    
    public Either<B, R> MapLeft<B>(Func<L, B> f) =>
        new (Morphism.MapLeft(f));

    public Either<X, Y> BiMap<X, Y>(Func<L, X> Left, Func<R, Y> Right) =>
        new(Morphism.BiMap(Left, Right));

    public Either<L, B> Select<B>(Func<R, B> f) =>
        new (Morphism.MapRight(f));

    public Either<L, B> Bind<B>(Func<R, Either<L, B>> f) =>
        new (Morphism.Bind(x => f(x).Morphism));

    public Either<L, C> SelectMany<B, C>(Func<R, Either<L, B>> bind, Func<R, B, C> f) =>
        Bind(x => bind(x).MapRight(y => f(x, y)));

    public A Match<A>(Func<L, A> Left, Func<R, A> Right, Func<A>? Bottom) =>
        Morphism.Match(Left, Right, Bottom);
}


public record Aff<Env, A>(EitherT<MIO<Env>, Env, Error, A> Transformer) 
{
    public Either<Error, A> Run(Env env) =>
        Transformer.Morphism.Invoke1(env) switch
        {
            TComplete<SumTransducer<Unit, Error, Unit, A>> c => 
                c.Value.Invoke1(Sum<Unit, Unit>.Right(default)) switch
                {
                    TComplete<Sum<Error, A>> { Value: SumRight<Error, A> r } => Either<Error, A>.Right(r.Value),
                    TComplete<Sum<Error, A>> { Value: SumLeft<Error, A> l }  => Either<Error, A>.Left(l.Value),
                    TFail<Sum<Error, A>> f                                   => Either<Error, A>.Left(f.Error), 
                    TCancelled<Sum<Error, A>>                                => Either<Error, A>.Left(Errors.Cancelled), 
                    _                                                        => Either<Error, A>.Left(Errors.None)
                },
            TFail<SumTransducer<Unit, Error, Unit, A>> f     => Either<Error, A>.Left(f.Error), 
            TCancelled<SumTransducer<Unit, Error, Unit, A>>  => Either<Error, A>.Left(Errors.Cancelled), 
            _                                                => Either<Error, A>.Left(Errors.None)
        };
}

public static class EitherTests
{
    public static void Foo()
    {
        var mx = Either<string, int>.Left("hello");
        var my = Either<string, int>.Right(100);
        var mz = Either<string, int>.Right(200);
        
        /*var mr = from x in mx 
                 from y in my
                 select x + y;#1#

    }
}*/