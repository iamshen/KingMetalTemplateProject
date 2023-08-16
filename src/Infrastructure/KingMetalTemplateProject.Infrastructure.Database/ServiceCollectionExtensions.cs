using System.Reflection;
using System.Runtime.CompilerServices;
using KingMetal.Infrastructures.Common.Utilities;
using KingMetalTemplateProject.Infrastructure.Database.Options;
using KingMetalTemplateProject.Infrastructure.Database.Repository;
using KingMetalTemplateProject.Infrastructure.Shared.Repository;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace KingMetalTemplateProject.Infrastructure.Database.Extensions;

public static class ServiceCollectionExtensions
{
    #region 获取数据库连接对象

    /// <summary>
    ///     获取数据库连接对象
    /// </summary>
    /// <param name="service"> </param>
    /// <returns> </returns>
    public static AppDataConnection GetAppDataConnection(this IServiceProvider service)
    {
        return service.GetService<AppDataConnection>() ?? throw new ArgumentException(nameof(AppDataConnection));
    }

    #endregion 获取数据库连接对象

    #region 添加数据库

    /// <summary>
    ///     添加数据库
    /// </summary>
    /// <param name="services"> </param>
    /// <param name="config">   配置委托 </param>
    /// <returns> </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IServiceCollection AddAppDataConnection(this IServiceCollection services,
        Action<AppDataBaseOptions> config)
    {
        var dbOptions = new AppDataBaseOptions();
        config(dbOptions);
        services.AddLinqToDBContext<AppDataConnection>((provider, options) =>
        {
            options.UsePostgreSQL(dbOptions.ConnectionString);

            var environment = provider.GetService<IHostEnvironment>();
            if (environment != null && !environment.IsProduction()) options.UseDefaultLogging(provider);

            return options;
        }, ServiceLifetime.Singleton);

        services.Replace(new ServiceDescriptor(typeof(AppDataConnection), provider =>
        {
            var options = provider.GetService<DataOptions<AppDataConnection>>();
            return options is null
                ? new AppDataConnection(dbOptions.ConnectionString)
                : new AppDataConnection(options.Options);
        }, ServiceLifetime.Transient));

        // 注册 AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        // 注册仓储
        services.TryAddScoped(typeof(IQueryBaseRepository<>), typeof(QueryBaseRepository<>));

        return services;
    }

    /// <summary>
    ///     添加数据库
    /// </summary>
    /// <param name="services">      </param>
    /// <param name="configuration"> 配置对象 </param>
    /// <returns> </returns>
    public static IServiceCollection AddAppDataConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddAppDataConnection(m =>
        {
            var cfg = ConfigurationHelper.GetConfiguration(configuration, nameof(AppDataBaseOptions));
            var connectionString = cfg.GetValue<string>(nameof(AppDataBaseOptions.ConnectionString));
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("ConnectionString Can Not Be Null");
            m.ConnectionString = connectionString;
        });
    }

    #endregion 添加数据库
}