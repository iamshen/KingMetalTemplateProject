using Ardalis.GuardClauses;
using GoldCloud.Infrastructure.Common.Options;
using HttpTracer;
using KingMetalTemplateProject.Infrastructure.HttpService.Apis;
using KingMetalTemplateProject.Infrastructure.HttpService.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> Add Application HttpClient  </summary>
    public static void AddAppHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        #region Configure

        services.Configure<RemoteApiConfigOptions>(configuration.GetSection(nameof(RemoteApiConfigOptions)));

        #endregion Configure

        #region RefitClient

        var baseAddress = configuration.GetSection("RemoteApiConfigOptions").GetValue<string>("BaseUrl");
        Guard.Against.NullOrWhiteSpace(baseAddress, message: "检查错误: 远程调用地址配置为空");
        var baseUri = new Uri(baseAddress);
        // 短信消息服务 Api
        services.AddRefitClient<IMessageApi>(config => config.BaseAddress = baseUri)
            .ConfigurePrimaryHttpMessageHandler(MessageHandler);

        // Others Api...

        #endregion RefitClient

        #region Services

        services.AddTransient<IMessageHttpService, MessageHttpService>();

        #endregion Services

        #region HttpClient

        // 
        //  推送重试策略   https://www.apifox.cn/apidoc/shared-dc9f8511-0ba2-439d-8272-a6fd40f3f909/doc-2120039
        // services.AddHttpClient(nameof(XXXService))
        //     .ConfigurePrimaryHttpMessageHandler(messageHandler)
        //     .AddPolicyHandler(Policy
        //         .Handle<Exception>()
        //         .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
        //         .WaitAndRetryAsync(3, retryAttempt => retryAttempt switch
        //         {
        //             1 => TimeSpan.FromSeconds(10),
        //             2 => TimeSpan.FromMinutes(5),
        //             3 => TimeSpan.FromMinutes(10),
        //             _ => TimeSpan.FromMinutes(1)
        //         })
        //         .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5))));

        #endregion

        HttpMessageHandler MessageHandler()
        {
            return new HttpTracerHandler { Verbosity = HttpMessageParts.All };
        }
    }
}