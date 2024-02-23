using Golden.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

namespace KingMetalTemplateProject.Infrastructure.Shared.Dtos.OperatorRecord;

/// <summary>
///     分页查询操作记录
/// </summary>
public class GetOperatorListDto: PageRequest
{
    /// <summary>
    ///     操作人Id
    /// </summary>
    public string? OperatorId { get; set; }

    /// <summary>
    ///     操作人姓名
    /// </summary>
    public string? OperatorUserName { get; set; }

    /// <summary>
    ///     操作人姓名
    /// </summary>
    public string? OperatorRealName { get; set; }

    /// <summary>
    ///     操作业务类型
    /// </summary>
    public BizType? BizType { get; set; }

    /// <summary>
    ///     ActorId
    /// </summary>
    public long ActorId { get; set; }

    /// <summary>
    ///     操作子项Id, 例如操作某个子项的Id
    /// </summary>
    public long SubjectId { get; set; }
}
