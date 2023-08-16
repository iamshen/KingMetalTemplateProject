using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KingMetalTemplateProject.Infrastructure.DatabaseTests;

/// <summary>
///     Test Fixture
/// </summary>
public class DependencySetupFixture
{
    /// <summary>
    ///     ctor
    /// </summary>
    public DependencySetupFixture()
    {
        var serviceCollection = new ServiceCollection();

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configurationBuilder.AddJsonFile("appsettings.json", false, true);
        var configuration = configurationBuilder.Build();

        serviceCollection.AddAppDataConnection(configuration);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    ///     ServiceProvider
    /// </summary>
    public IServiceProvider ServiceProvider { get; private set; }
}