using KingMetalTemplateProject.Application.Services;
using KingMetalTemplateProject.Domain.Interfaces.Services;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>  ServiceCollection </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add Application ApiServices </summary>
    public static IServiceCollection AddAppApiServices(this IServiceCollection services)
    {
        // Api 服务
        services.AddScoped<ICommonApiService, CommonApiService>();
        services.AddScoped<IOperatorRecordApiService, OperatorRecordApiService>();

        return services;
    }
}