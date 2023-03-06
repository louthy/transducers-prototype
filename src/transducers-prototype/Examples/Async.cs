using LanguageExt.HKT;

namespace LanguageExt.Examples;

public record Async<Env, A>(Transducer<Env, ValueTask<A>> MorphismValue) : 
    KArr<Async<Env>, Env, A>
{
    public Transducer<Env, A> Morphism =>
        new AsyncTransducer<Env, A>(MorphismValue);
}

public readonly struct Async<Env> : MonadSum<Async<Env>, Env, ValueTask<Error>>
{
}