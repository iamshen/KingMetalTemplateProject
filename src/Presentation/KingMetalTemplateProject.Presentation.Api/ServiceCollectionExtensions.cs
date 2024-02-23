namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     服务扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     配置应用服务
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    /// <returns></returns>
    public static void ConfigurationAppServices(this WebApplicationBuilder webApplicationBuilder)
    {
        var section = webApplicationBuilder.Configuration.GetSection(nameof(RemoteApiConfigOptions));
        webApplicationBuilder.Services.Configure<RemoteApiConfigOptions>(section);
        webApplicationBuilder.Services.AddDistributedMemoryCache();
        webApplicationBuilder.Services.AddHttpContextAccessor();
        webApplicationBuilder.Services.AddAppApiServices();
        webApplicationBuilder.Services.AddAppDataConnection(webApplicationBuilder.Configuration);
        webApplicationBuilder.Services.AddAppHttpClients(webApplicationBuilder.Configuration);
    }
}