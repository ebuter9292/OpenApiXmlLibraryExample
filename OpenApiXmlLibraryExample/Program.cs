using Asp.Versioning;
using OpenApiHelper;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
List<ApiVersion> versions = [new ApiVersion(1, 0), new ApiVersion(2, 0)];
builder.Services.AddCustomisedOpenApi(versions);
//builder.Services.AddOpenApi();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwaggerUI(options =>
{
    //options.SwaggerEndpoint("/openapi/v1.json", "v1");
    foreach (ApiVersion apiVersion in versions)
    {
        options.SwaggerEndpoint($"/openapi/{apiVersion}.json", $"example v{apiVersion}");
    }
});

app.Run();
