namespace SS14.GithubApiHelper.Configuration;

public sealed class GithubConfiguration
{
    public const string Name = "Github";

    public bool Enabled { get; set; } = true;
    public string? AppName { get; set; }
    public string? AppPrivateKeyLocation { get; set; }
    public int? AppId { get; set; }
    public string? TemplateLocation { get; set; }
}
