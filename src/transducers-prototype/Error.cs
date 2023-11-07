#nullable enable
namespace LanguageExt;

public readonly record struct Error(string Message)
{
    public static implicit operator Error(Exception e) =>
        new Error(e.Message);

    public A Throw<A>() =>
        throw new Exception(Message);

    public static Error New(Exception e) =>
        new Error(e.Message);

    public static Error New(string message) =>
        new Error(message);
}

public static class Errors
{
    public static readonly Error Cancelled = new ("Cancelled");
    public static readonly Error None = new ("None");
}