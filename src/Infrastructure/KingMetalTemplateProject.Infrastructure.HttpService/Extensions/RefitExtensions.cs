using System.Text.Json;
using Refit;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// </summary>
internal static class RefitExtensions
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static IHttpClientBuilder AddRefitClient<T>(
        this IServiceCollection services,
        Action<HttpClient> configureClient,
        RefitSettings? settings = null)
        where T : class
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());

        if (settings is null)
            settings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions)
            };
        else
            settings.ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions);

        return services.AddRefitClient<T>(settings)
            .ConfigureHttpClient(configureClient);
    }
}