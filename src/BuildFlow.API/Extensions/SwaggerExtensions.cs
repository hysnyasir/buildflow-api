using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildFlow.API.Extensions;

/// <summary>
/// Generates one Swagger document per discovered API version.
/// Injected as IConfigureOptions so it runs after
/// IApiVersionDescriptionProvider is available.
/// </summary>
public sealed class ConfigureSwaggerOptions
    : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateOpenApiInfo(description));
        }

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter your JWT token.\n\nExample: eyJhbGciOiJIUzI1NiIsInR5..."
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                []
            }
        });
    }

    private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "BuildFlow API",
            Version = description.ApiVersion.ToString(),
            Description = "BuildFlow — SaaS Construction Management System",
            Contact = new OpenApiContact
            {
                Name = "BuildFlow Support",
                Email = "support@buildflow.io"
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " [This API version is deprecated]";
        }

        return info;
    }
}

/// <summary>
/// Extension methods for registering and using Swagger in the application.
/// </summary>
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();
        return services;
    }

    public static IApplicationBuilder UseSwaggerWithVersioning(
        this IApplicationBuilder app,
        IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"BuildFlow API {description.GroupName.ToUpperInvariant()}");
            }

            options.RoutePrefix = "swagger";
            options.DocumentTitle = "BuildFlow API";
        });

        return app;
    }
}
