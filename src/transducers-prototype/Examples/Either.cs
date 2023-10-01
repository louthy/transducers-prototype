using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Either<L, R>(EitherT<Identity<Unit>, Unit, L, R> Transformer)
{
    public static Either<L, R> Right(R value) =>
        new (EitherT<Identity<Unit>, Unit, L, R>.Right(value));
    
    public static Either<L, R> Left(L value) =>
        new (EitherT<Identity<Unit>, Unit, L, R>.Left(value));
}

public record Eff<Env, A>(EitherT<IO<Env>, Env, Error, A> Transformer)
{
}

public record Aff<Env, A>(EitherT<Async<Env>, Env, Error, A> Transformer) 
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
        
        var mr = from x in mx 
                 from y in my
                 select x + y;

    }
}