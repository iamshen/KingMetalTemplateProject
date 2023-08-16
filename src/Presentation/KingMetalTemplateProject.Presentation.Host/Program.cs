using System.Globalization;
using GoldCloud.Infrastructure.Common.Options;
using GoldCloud.Infrastructure.HostExtensions.Extensions;
using KingMetal.Domains.UniqueValueService.Grains;
using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Orleans.Streams;

namespace GoldCloud.Presentation.PromotionHost;

internal class Program
{
    #region 主程序入口

    /// <summary>
    ///     主程序入口
    /// </summary>
    /// <returns> </returns>
    public static Task Main(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .AddRemoteConfiguration()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration(builder => builder.AddUserSecrets<Program>())
            .ConfigurationKingMetalDefault(b =>
            {
                b.AddSimpleMessageStreamProvider(ApplicationConstants.StreamProviderName, options =>
                    {
                        options.FireAndForgetDelivery = true;
                        options.PubSubType = StreamPubSubType.ExplicitGrainBasedOnly;
                    })
                    .AddMemoryGrainStorage(ApplicationConstants.MemoryGrainStorageName)
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(UniqueValueServiceGrain).Assembly).WithReferences();
                        // TODO: 
                        // parts.AddApplicationPart(typeof(XXXGrain).Assembly).WithReferences();
                        // parts.AddApplicationPart(typeof(XXXDbGrain).Assembly).WithReferences();
                    })
                    .ConfigureServices((_, _) =>
                    {
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-hans");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-hans");
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var section = context.Configuration.GetSection(nameof(RemoteApiConfigOptions));
                        services.Configure<RemoteApiConfigOptions>(section);
                        services.AddSingleton(context.Configuration);
                        services.AddAppDataConnection(context.Configuration);
                        services.AddAppHttpClient(context.Configuration);
                    })
                    ;
            })
            .Build()
            .UseApplicationStarted()
            .UseApplicationStopped()
            .UseSafeStopSlioHost()
            .RunAsync();
    }

    #endregion 主程序入口
}
