using System.Runtime.CompilerServices;
using AutoMapper;
using Golden.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.Shared.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Orleans;
using RequestClientInfo = KingMetal.Domains.Abstractions.ValueObject.RequestClientInfo;

namespace KingMetalTemplateProject.Application.Services;

/// <summary> Base Api Service </summary>
public class BaseApiService
{
    /// <summary>服务提供者</summary>
    public readonly IServiceProvider ServiceProvider;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="provider"></param>
    public BaseApiService(IServiceProvider provider)
    {
        ServiceProvider = provider;
    }

    /// <summary> IMapper/ </summary>
    public IHttpContextAccessor HttpContextAccessor => ServiceProvider.GetRequiredService<IHttpContextAccessor>();

    /// <summary> IMapper/ </summary>
    public IMapper Mapper => ServiceProvider.GetRequiredService<IMapper>();

    /// <summary>集群客户端</summary>
    public IClusterClient ClusterClient => ServiceProvider.GetRequiredService<IClusterClient>();

    /// <summary>日志对象</summary>
    public ILogger Logger => ServiceProvider.GetRequiredService<ILogger<BaseApiService>>();

    /// <summary> 操作人 </summary>
    public OperatorObject<long>? OperatorObject => HttpContextAccessor.HttpContext?.GetOperatorObject();

    /// <summary>
    ///     客户端请求信息
    /// </summary>
    public RequestClientInfo? RequestClientInfo
    {
        get
        {
            var clientInfo = HttpContextAccessor.HttpContext?.GetClientInfo();
            if (clientInfo is null) return null;

            return new RequestClientInfo
            {
                Ip = clientInfo.Ip, UserAgent = clientInfo.UserAgent, HostInfo = clientInfo.HostInfo
            };
        }
    }

    /// <summary>  获取数据库仓储 </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IQueryBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
    {
        return ServiceProvider.GetRequiredService<IQueryBaseRepository<TEntity>>();
    }
}