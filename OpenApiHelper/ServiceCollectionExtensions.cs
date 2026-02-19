using Asp.Versioning;
using MartinCostello.OpenApi;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace OpenApiHelper;

public static class ServiceCollectionExtensions
{
    public static void AddCustomisedOpenApi<T>(this IServiceCollection services, List<ApiVersion> versions)
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
            services.AddOpenApiExtensions(apiVersion.ToString(), (options) =>
            {
                //options.AddExamples = true;
                options.AddXmlComments<T>();
            });
        }

    }

    public static void AddBasicOpenApi<T>(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddOpenApiExtensions((options) =>
        {
            //options.AddExamples = true;
            options.AddXmlComments<T>();
        });
    }

    public static void AddWeirdOpenApi(this IServiceCollection services, Action<IServiceCollection, string, Action<OpenApiOptions>> addOpenApiAction, List<ApiVersion> versions)
    {
        foreach (ApiVersion apiVersion in versions)
        {
            addOpenApiAction.Invoke(services, apiVersion.ToString(), options => new OpenApiOptions
            {
                ShouldInclude = (apiDesc) =>
                {
                    var endpointVersion = (ApiVersion)apiDesc.Properties[typeof(ApiVersion)];
                    if (endpointVersion is null)
                        return true;
                    if (endpointVersion == apiVersion)
                        return true;
                    return false;
                }
            });
        }
    }
}