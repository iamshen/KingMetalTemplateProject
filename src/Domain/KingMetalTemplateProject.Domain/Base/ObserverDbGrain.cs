// -----------------------------------------------------------------------
//  <last-editor>黄深</last-editor>
//  <last-date>2022-05-30 17:00</last-date>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using KingMetal.Domains.Abstractions.Event;
using KingMetal.Domains.Core.Grains;
using KingMetalTemplateProject.Domain.Interfaces.Events.Common;
using KingMetalTemplateProject.Infrastructure.Database;
using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using LinqToDB;
using Microsoft.Extensions.Logging;
using Orleans;

namespace KingMetalTemplateProject.Domain.Observers;

/// <summary>
///     DbGrain
/// </summary>
public abstract class ObserverDbGrain : ObserverGrain<long>
{
    /// <inheritdoc />
    protected override Task Initialize()
    {
        EventHandlerRetryCount = 5;

        return base.Initialize();
    }

    #region 获取数据库对象

    /// <summary>
    ///     获取数据库对象
    /// </summary>
    /// <returns> </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual AppDataConnection GetDataConnection()
    {
        return ServiceProvider.GetAppDataConnection();
    }

    #endregion 获取数据库对象

    #region 操作记录

    /// <summary>
    ///     操作记录
    /// </summary>
    /// <param name="event"></param>
    /// <param name="eventMetadata"></param>
    /// <returns></returns>
    public async Task Handler(OperationRecordAddedEvent @event, EventMetadata eventMetadata)
    {
        await using var db = GetDataConnection();

        try
        {
            var id = await GrainFactory.NewIntegerIdAsync();

            var @operator = @event.OperationAudit.Operator;

            await db.InsertAsync(new GcTOperateRecord
            {
                Id = id,
                OperatorId = @operator.OperatorId,
                OperatorUserName = @operator.OperatorUserName,
                OperatorRealName = @operator.OperatorRealName,
                BizType = @event.OperationAudit.BizType,
                ActorId = ActorId,
                RequestClientInfo = @event.OperationAudit.ClientInfo,
                AttachData = @event.OperationAudit.AttachData,
                SubjectId = @event.OperationAudit.SubjectId,
                SubjectRemark = @event.OperationAudit.SubjectRemark,
                Remark = @event.OperationAudit.Remark,
                OperationTime = @event.OperationAudit.OperationTime
            });

            LogInfo(eventMetadata, @event);
        }
        catch (Exception ex)
        {
            LogError(ex, eventMetadata, @event);
        }
    }

    #endregion

    #region 打印错误日志

    /// <summary>
    ///     打印错误日志
    /// </summary>
    /// <param name="ex">       异常信息 </param>
    /// <param name="eventMetadata">   事件元数据 </param>
    /// <param name="event"> 事件 </param>
    protected virtual void LogError(Exception ex, EventMetadata eventMetadata, IEvent @event)
    {
        Logger.LogError(ex, "{DefaultName} Fail; EventId:{EventMetadataEventId}. Message: {ExMessage}",
            @event.GetDefaultName(), eventMetadata.EventId, ex.Message);
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
}