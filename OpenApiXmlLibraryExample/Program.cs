using Asp.Versioning;
using OpenApiHelper;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


bool useVersioning = true;
bool useExternalOpenApiRegistration = true;
bool useWeirdOpenApiRegistration = false;


List<ApiVersion> versions = [new(1, 0), new(2, 0)];

if (useExternalOpenApiRegistration)
{
    if (useWeirdOpenApiRegistration)
    {
        builder.Services.AddWeirdOpenApi((services, name, options) => services.AddOpenApi(name, options), versions);

    }
    else
    {
        if (useVersioning)
            builder.Services.AddCustomisedOpenApi<Program>(versions);
        else
            builder.Services.AddBasicOpenApi<Program>();
    }
}
else
{
    if (useVersioning)
    {
        foreach (ApiVersion apiVersion in versions)
        {
            builder.Services.AddOpenApi(apiVersion.ToString(), options =>
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
    else
    {
        builder.Services.AddOpenApi();
    }
}


    builder.Services
        .AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddMvc(options => { })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwaggerUI(options =>
{
    if (useVersioning)
    {
        foreach (ApiVersion apiVersion in versions)
            options.SwaggerEndpoint($"/openapi/{apiVersion}.json", $"example v{apiVersion}");
    }
    else
    {
        options.SwaggerEndpoint("/openapi/v1.json", "example");
    }
});

app.Run();