namespace LanguageExt;

public static class Result
{
    public static Result<A> Value<A>(A value) =>
        new ResultValue<A>(value);
    
    public static Result<A> Fail<A>(Error value) =>
        new ResultFail<A>(value);
    
}

public abstract record Result<A>
{
    public static readonly Result<A> Cancelled = new ResultFail<A>(Errors.Cancelled);
}

public record ResultValue<A>(A Value) : Result<A>;
public record ResultFail<A>(Error Error) : Result<A>;
