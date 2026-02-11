using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace OpenApiHelper;

public static class ServiceCollectionExtensions
{
    public static void AddCustomisedOpenApi(this IServiceCollection services, List<ApiVersion> versions)
    {
        foreach (ApiVersion apiVersion in versions)
        {
            services.AddOpenApi(apiVersion.ToString(), options =>
            {
                options.ShouldInclude = (apiDesc) =>
                {
                    var endpointVersion = (ApiVersion)apiDesc.Properties[typeof(ApiVersion)];
                    if (endpointVersion is null)
                        return true;
                    if (endpointVersion == apiVersion)
                        return true;
                    return false;
                };
            });
        }
    }
}