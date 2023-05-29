namespace SS14.MapServer.Exceptions;

public sealed class ConfigurationException : Exception
{
    public ConfigurationException(string? message) : base(message)
    {
    }
}