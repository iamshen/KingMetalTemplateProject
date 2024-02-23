namespace KingMetalTemplateProject.PresentationTests;

/// <summary>
///     Test SetupFixture
/// </summary>
public class ClusterFixture : IDisposable
{
    /// <summary>
    ///     ctor
    /// </summary>
    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();

        builder.AddSiloBuilderConfigurator<TestSiloConfigurator>();
        builder.AddClientBuilderConfigurator<TestClientConfigurator>();

        Cluster = builder.Build();
        Cluster.Deploy();
    }

    /// <summary>  Cluster </summary>
    public TestCluster Cluster { get; }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }
}

public class TestClientConfigurator : IClientBuilderConfigurator
{
    /// <summary>
    ///     ctor
    /// </summary>
    public TestClientConfigurator()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }

    /// <summary> IConfiguration </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.ConfigureLogging(logging => logging.AddConsole());
        clientBuilder.ConfigureServices((_, services) =>
        {
            services.AddHttpContextAccessor();
            services.AddAppApiServices();
            services.AddAppDataConnection(Configuration);
            services.AddAppHttpClients(Configuration);
        });
    }
}

public class TestSiloConfigurator : ISiloConfigurator
{
    /// <summary>
    ///     ctor
    /// </summary>
    public TestSiloConfigurator()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        HostingEnvironment = new HostingEnvironment
        {
            ApplicationName = AppDomain.CurrentDomain.FriendlyName,
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
            ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory)
        };
    }

    /// <summary> IConfiguration </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary> IHostEnvironment </summary>
    public static IHostEnvironment HostingEnvironment { get; private set; } = null!;

    public void Configure(ISiloBuilder siloBuilder)
    {
        bool flag = true;
        //  Consul 集群
        if (!flag)
        {
           siloBuilder.UseConsulClustering(HostingEnvironment, Configuration);
        }
        // 本地集群
        siloBuilder.UseLocalhostClustering(34410, 35510, null, "unitTest", "unitTest");
        
        siloBuilder.UseAdoNetReminderService(Configuration);
        siloBuilder.AddKingMetal(Configuration);
        siloBuilder.AddKingMetalConsumer();
        siloBuilder.AddUniqueIdService();
        siloBuilder.AddUniqueValueService();
        siloBuilder.AddTransactionService(Configuration);
        siloBuilder.AddAuditingService(Configuration);
        siloBuilder.AddPostgreSQLEventStorage(Configuration);
        siloBuilder.AddPostgreSQLSnapshotStorage(Configuration);
        siloBuilder.AddPostgreSQLCommandStorage(Configuration);
        siloBuilder.AddPostgreSQLObserverStateStorage(Configuration);
        siloBuilder.AddRabbitMQMessagBus(Configuration);
        siloBuilder.AddPostgreSQLTransactionStorage(Configuration);
        siloBuilder.AddPostgreSQLUniqueIdStorage(Configuration);
        siloBuilder.AddPostgreSQLUniqueValueStorage(Configuration);
        siloBuilder.AddPostgreSQLAuditingStorage(Configuration);
        siloBuilder.AddKingMetalMemoryStoragee();
        siloBuilder.ConfigureServices(m => m.AddMemoryCache());

        siloBuilder.AddSimpleMessageStreamProvider(ApplicationConstants.StreamProviderName, options =>
        {
            options.FireAndForgetDelivery = true;
            options.PubSubType = StreamPubSubType.ExplicitGrainBasedOnly;
        });
        siloBuilder.AddMemoryGrainStorage(ApplicationConstants.MemoryGrainStorageName);
        siloBuilder.ConfigureApplicationParts(parts =>
        {
            parts.AddApplicationPart(typeof(UniqueValueServiceGrain).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(KingMetalTemplateProject.Domain.Grain.DomainGrain<>).Assembly).WithReferences();
            parts.AddApplicationPart(typeof(ObserverDbGrain).Assembly).WithReferences();
        });
        siloBuilder.ConfigureServices((_, _) =>
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-hans");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-hans");
        });
        siloBuilder.ConfigureServices((_, services) =>
        {
            services.Configure<RemoteApiConfigOptions>(Configuration.GetSection(nameof(RemoteApiConfigOptions)));
            services.AddSingleton(Configuration);
            services.AddAppDataConnection(Configuration);
            services.AddAppHttpClients(Configuration);
        });
        siloBuilder.ConfigureLogging((hostContext, loggerBuilder) =>
        {
            loggerBuilder.ClearProviders();
            loggerBuilder.AddConsole();
            loggerBuilder.AddDebug();
            loggerBuilder.AddNLogKingMetalRenderer();
            loggerBuilder.AddNLogForKingMetal(hostContext.Configuration);
        });
    }
}
