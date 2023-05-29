namespace SS14.MapServer.Configuration;

public sealed class GithubConfiguration
{
    public const string Name = "Github";

    public bool Enabled { get; set; } = true;
    public string? AppName { get; set; }
    public string? AppPrivateKeyLocation { get; set; }
    public int? AppId { get; set; }
}