using KingMetal.Domains.Abstractions.Attributes;
using KingMetal.Domains.Abstractions.Event;
using KingMetalTemplateProject.Infrastructure.Shared.ValueObjects;

namespace KingMetalTemplateProject.Domain.Interfaces.Events.Common;

#region 操作记录

/// <summary>
///     操作记录
/// </summary>
[Event(Name = nameof(OperationRecordAddedEvent))]
public class OperationRecordAddedEvent : IEvent
{
    #region 属性

    /// <summary>
    ///     操作审计
    /// </summary>
    public OperationAuditObject OperationAudit { get; init; } = null!;

    #endregion 属性
}

#endregion