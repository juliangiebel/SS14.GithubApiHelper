namespace SS14.GithubApiHelper.Exceptions;

public sealed class RateLimitException : Exception
{
    public RateLimitException(string? message) : base(message)
    {
    }
}