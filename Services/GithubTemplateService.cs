using System.Diagnostics;
using System.Globalization;
using Fluid;
using Microsoft.Extensions.Configuration;
using Serilog;
using SS14.GithubApiHelper.Configuration;

namespace SS14.GithubApiHelper.Services;

public sealed class GithubTemplateService
{
    private const string TemplateFileSearchPattern = "*.liquid";

    private readonly FluidParser _parser;

    private readonly GithubConfiguration _configuration = new();
    private readonly Dictionary<string, IFluidTemplate> _templates = new();
    private readonly ILogger _log;

    public GithubTemplateService(FluidParser parser, IConfiguration configuration)
    {
        _parser = parser;
        configuration.Bind(GithubConfiguration.Name, _configuration);
        _log = Log.ForContext<GithubTemplateService>();
    }

    public async Task LoadTemplates()
    {
        var path = _configuration.TemplateLocation;

        if (path == null)
        {
            _log.Error("Tried to load templates without template path configured [Github.TemplateLocation]");
            return;
        }

        if (!Directory.Exists(path))
        {
            _log.Error("Template path doesn't exist: {TemplatePath}", path);
            return;
        }

        _log.Information("Preloading github message templates");

        var directory = new DirectoryInfo(path);

        var enumerationOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            MaxRecursionDepth = 5
        };

        foreach (var templateFile in directory.EnumerateFiles(TemplateFileSearchPattern, enumerationOptions))
        {
            var rawTemplate = await File.ReadAllTextAsync(templateFile.FullName);
            if (!_parser.TryParse(rawTemplate, out var template, out var error))
            {
                _log.Error(
                    "Failed to parse template: {TemplateName}.\n{ErrorMessage}",
                    templateFile.Name,
                    error
                    );
                continue;
            }

            var templateName = Path.GetRelativePath(path, templateFile.FullName);
            templateName = templateName.Replace(Path.GetExtension(templateName), "");
            _templates.Add(templateName, template);
        }

        _log.Information("Loaded {TemplateCount} templates", _templates.Count);
    }

    public async Task<string> RenderTemplate(string templateName, object? model = null, CultureInfo? culture = null)
    {
        model ??= new { };
        culture ??= CultureInfo.InvariantCulture;

        if (!_templates.TryGetValue(templateName, out var template))
        {
            _log.Error("No template with name: {TemplateName}", templateName);
            return "";
        }

        var context = new TemplateContext(model)
        {
            CultureInfo = culture,
            Options =
            {
                Greedy = true
            }
        };

        return await template.RenderAsync(context);
    }
}
