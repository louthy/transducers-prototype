#nullable enable
namespace LanguageExt;

public static class TResultAsync
{
    public static TResultAsync<A> Continue<A>(A value) => new TContinueAsync<A>(value);
    public static TResultAsync<A> Complete<A>(A value) => new TCompleteAsync<A>(value);
    public static TResultAsync<A> Fail<A>(Error Error) => new TFailAsync<A>(Error);
    public static TResultAsync<A> Cancel<A>() => TCancelledAsync<A>.Default;
    public static TResultAsync<A> None<A>() => TNoneAsync<A>.Default;
    
    public static TResultAsync<S> Recursive<S, A>(TState st, S s, A value, ReducerAsync<S, A> reduce) => 
        new TRecursiveAsync<S>(new TRecursiveReduceAsync<S, A>(st, s, value, reduce));
    
    public static TResultAsync<S> Recursive<S>(TRecursiveRunnerAsync<S> runner) => 
        new TRecursiveAsync<S>(runner);
}

public abstract record TResultAsync<A>
{
    public abstract bool Success { get; }
    public abstract bool Continue { get; }
    public abstract bool Faulted { get; }
    public abstract bool Recursive { get; }
    public virtual A ValueUnsafe => throw new InvalidOperationException("Can't call ValueUnsafe on a TResultAsync that has no value");
    public virtual Error ErrorUnsafe => throw new InvalidOperationException("Can't call ErrorUnsafe on a TResultAsync that succeeded");
    public abstract TResultAsync<B> Map<B>(Func<A, B> f);
    public abstract TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> f);
    public abstract ValueTask<TResultAsync<S>> Reduce<S>(TState state, S stateValue, ReducerAsync<S, A> reducer);

    public static implicit operator ValueTask<TResultAsync<A>>(TResultAsync<A> x) =>
        new(x);
}
public record TContinueAsync<A>(A Value) : TResultAsync<A>
{
    public override bool Success => true;
    public override bool Continue => true;
    public override bool Faulted => false;
    public override bool Recursive => false;
    public override A ValueUnsafe => Value;

    public override TResultAsync<B> Map<B>(Func<A, B> f) =>
        TResultAsync.Continue(f(Value));

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> f) =>
        f(Value);

    public override ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r) =>
        new(TResultAsync.Continue(s));
                    
    public override string ToString() =>
        $"Continue({Value})";
}

public record TCompleteAsync<A>(A Value) : TResultAsync<A>
{
    public override bool Success => true;
    public override bool Continue => false;
    public override bool Faulted => false;
    public override bool Recursive => false;
    public override A ValueUnsafe => Value;

    public override TResultAsync<B> Map<B>(Func<A, B> f) =>
        TResultAsync.Complete(f(Value));

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> f) =>
        f(Value);

    public override ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r) =>
        new(TResultAsync.Recursive(st, s, Value, r));
                    
    public override string ToString() =>
        $"Complete({Value})";
}
public record TCancelledAsync<A> : TResultAsync<A>
{
    public static readonly TResultAsync<A> Default = new TCancelledAsync<A>();
    
    public override bool Success => false;
    public override bool Continue => false;
    public override bool Faulted => true;
    public override bool Recursive => false;
    public override Error ErrorUnsafe => Errors.Cancelled;

    public override TResultAsync<B> Map<B>(Func<A, B> _) =>
        TCancelledAsync<B>.Default;

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> _) =>
        TCancelledAsync<B>.Default;

    public override ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r) =>
        new(TCancelledAsync<S>.Default);
                
    public override string ToString() =>
        "Cancelled";
}
public record TNoneAsync<A> : TResultAsync<A>
{
    public static readonly TResultAsync<A> Default = new TNoneAsync<A>();
    
    public override bool Success => true;
    public override bool Continue => false;
    public override bool Recursive => false;
    public override bool Faulted => false;

    public override TResultAsync<B> Map<B>(Func<A, B> _) =>
        TNoneAsync<B>.Default;

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> _) =>
        TNoneAsync<B>.Default;

    public override ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r) =>
        new(TNoneAsync<S>.Default);
                    
    public override string ToString() =>
        $"None";
}
public record TFailAsync<A>(Error Error) : TResultAsync<A>
{
    public override bool Success => false;
    public override bool Continue => false;
    public override bool Faulted => true;
    public override bool Recursive => false;
    public override Error ErrorUnsafe => Error;

    public override TResultAsync<B> Map<B>(Func<A, B> _) =>
        TResultAsync.Fail<B>(Error);

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> _) =>
        TResultAsync.Fail<B>(Error);

    public override ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r) =>
        new(TResultAsync.Fail<S>(Error));
                
    public override string ToString() =>
        $"Fail({Error})";
}

public record TRecursiveAsync<A>(TRecursiveRunnerAsync<A> Runner) : TResultAsync<A>
{
    public override bool Success => false;
    public override bool Continue => false;
    public override bool Faulted => false;
    public override bool Recursive => true;

    public override TResultAsync<B> Map<B>(Func<A, B> f) =>
        new TRecursiveAsync<B>(Runner.Map(f));

    public override TResultAsync<B> Bind<B>(Func<A, TResultAsync<B>> f) =>
        new TRecursiveAsync<B>(Runner.Bind(f));

    public ValueTask<TResultAsync<A>> Run() =>
        Runner.Run();

    public override async ValueTask<TResultAsync<S>> Reduce<S>(TState st, S s, ReducerAsync<S, A> r)
    {
        var ra = await Runner.Run().ConfigureAwait(false);
        return ra switch
        {
            TContinueAsync<A> va => TResultAsync.Recursive(st, s, va.Value, r),
            TCompleteAsync<A> => TResultAsync.Complete(s),
            TFailAsync<A> f => TResultAsync.Fail<S>(f.Error),
            TCancelledAsync<A> => TResultAsync.Cancel<S>(),
            TNoneAsync<A> => TResultAsync.None<S>(),
            TRecursiveAsync<A> vr => vr.Bind(a => TResultAsync.Recursive(st, s, a, r)),
            _ => throw new NotSupportedException()
        };
    }
            
    public override string ToString() =>
        "Recursive";
}

public abstract record TRecursiveRunnerAsync<A>
{
    public abstract ValueTask<TResultAsync<A>> Run();

    public TRecursiveRunnerAsync<B> Map<B>(Func<A, B> f) =>
        new TRecursiveMapAsync<A, B>(this, f);
    
    public TRecursiveRunnerAsync<B> Bind<B>(Func<A, TResultAsync<B>> f) =>
        new TRecursiveBindAsync<A, B>(this, f);
}

public record TRecursiveReduceAsync<S, A>(TState State, S StateValue, A Value, ReducerAsync<S, A> Next) 
    : TRecursiveRunnerAsync<S>
{
    public override ValueTask<TResultAsync<S>> Run() =>
        Next.Run(State, StateValue, Value);
}

public record TRecursiveMapAsync<A, B>(TRecursiveRunnerAsync<A> Next, Func<A, B> F)
    : TRecursiveRunnerAsync<B>
{
    public override async ValueTask<TResultAsync<B>> Run() =>
        (await Next.Run().ConfigureAwait(false)).Map(F);
}

public record TRecursiveBindAsync<A, B>(TRecursiveRunnerAsync<A> Next, Func<A, TResultAsync<B>> F)
    : TRecursiveRunnerAsync<B>
{
    public override async ValueTask<TResultAsync<B>> Run() =>
        (await Next.Run().ConfigureAwait(false)).Bind(F);
}
