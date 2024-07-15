namespace SS14.GithubApiHelper.Exceptions;

public sealed class ConfigurationException : Exception
{
    public ConfigurationException(string? message) : base(message)
    {
    }
}