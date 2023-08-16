using System.Runtime.CompilerServices;
using KingMetal.Domains.Abstractions.Event;
using KingMetal.Domains.Core.Grains;
using KingMetalTemplateProject.Infrastructure.Database;
using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using Microsoft.Extensions.Logging;

namespace KingMetalTemplateProject.Domain.Observers;

/// <summary>
///     FlowGrain
/// </summary>
public abstract class ObserverFlowGrain : ObserverGrain<long>
{
    #region 获取数据库对象

    /// <summary>
    ///     获取数据库对象
    /// </summary>
    /// <returns> </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual AppDataConnection GetDataConnection()
    {
        return ServiceProvider.GetAppDataConnection()!;
    }

    #endregion 获取数据库对象

    #region 打印信息日志

    /// <summary>
    ///     打印信息日志
    /// </summary>
    /// <param name="eventMetadata">    事件元数据 </param>
    /// <param name="event">  事件 </param>
    protected virtual void LogInfo(EventMetadata eventMetadata, IEvent @event)
    {
        Logger.LogInformation("{DefaultName} Success; EventId:{EventMetadataEventId}", @event.GetDefaultName(),
            eventMetadata.EventId);
    }

    /// <summary>
    ///     打印信息日志
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="eventName"></param>
    protected virtual void LogInfo(string eventId, string eventName)
    {
        Logger.LogInformation("{EventName} Success; EventId:{EventId}", eventName, eventId);
    }

    #endregion 打印信息日志

    #region 打印错误日志

    /// <summary>
    ///     打印错误日志
    /// </summary>
    /// <param name="exc">       异常信息 </param>
    /// <param name="eventMetadata">   事件元数据 </param>
    /// <param name="event"> 事件 </param>
    protected virtual void LogError(Exception exc, EventMetadata eventMetadata, IEvent @event)
    {
        Logger.LogError(exc, "{DefaultName} Failed; EventId:{EventMetadataEventId}", @event.GetDefaultName(),
            eventMetadata.EventId);
    }

    /// <summary>
    ///     打印错误日志
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="eventId"></param>
    /// <param name="eventName"></param>
    protected virtual void LogError(Exception ex, string eventId, string eventName)
    {
        Logger.LogError(ex, "{EventName} Fail; EventId:{EventId}. Message: {ExMessage}", eventName, eventId,
            ex.Message);
    }

    #endregion 打印错误日志
}