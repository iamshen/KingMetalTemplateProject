using Golden.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

namespace KingMetalTemplateProject.Infrastructure.Shared.Dtos.OperatorRecord;

/// <summary>
///     操作记录输出Dto
/// </summary>
public class OperateRecordOutputDto
{
    /// <summary>
    ///     操作记录Id
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     操作人Id
    /// </summary>
    public string? OperatorId { get; set; }

    /// <summary>
    ///     操作人用户名
    /// </summary>
    public string? OperatorUserName { get; set; }

    /// <summary>
    ///     操作人姓名
    /// </summary>
    public string? OperatorRealName { get; set; }

    /// <summary>
    ///     操作业务类型
    /// </summary>
    public BizType BizType { get; set; }

    /// <summary>
    ///     操作业务类型名称
    /// </summary>
    public string BizTypeName => BizType.GetDisplayName();

    /// <summary>
    ///     请求客户端信息
    /// </summary>
    public RequestClientInfo? RequestClientInfo { get; set; }

    /// <summary>
    ///     ActorId
    /// </summary>
    public string ActorId { get; set; } = string.Empty;

    /// <summary>
    ///     附加数据
    /// </summary>
    public object? AttachData { get; set; }

    /// <summary>
    ///     操作子项Id, 例如操作某个子项的Id
    /// </summary>
    public string? SubjectId { get; set; }

    /// <summary>
    ///     操作子项说明, 例如操作某个子项的说明
    /// </summary>
    public string? SubjectRemark { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    public long OperationTime { get; set; }
}
