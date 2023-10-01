#nullable enable
namespace LanguageExt;

public readonly record struct Error(string Message)
{
    public static implicit operator Error(Exception e) =>
        new Error(e.Message);
}

public static class Errors
{
    public static readonly Error Cancelled = new ("Cancelled");
    public static readonly Error None = new ("None");
}