using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SS14.GithubApiHelper.Services;

namespace SS14.GithubApiHelper.Extensions;

public static class GithubTemplateServiceExtension
{
    public static void AddGithubTemplating(this IServiceCollection services)
    {
        services.AddSingleton<FluidParser>();
        services.AddSingleton<GithubTemplateService>();
    }

    public static async Task PreloadGithubTemplates(this WebApplication application)
    {
        var templateService = application.Services.GetService<GithubTemplateService>();
        await templateService?.LoadTemplates()!;
    }
}
