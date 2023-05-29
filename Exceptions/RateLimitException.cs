namespace SS14.MapServer.Exceptions;

public sealed class RateLimitException : Exception
{
    public RateLimitException(string? message) : base(message)
    {
    }
}