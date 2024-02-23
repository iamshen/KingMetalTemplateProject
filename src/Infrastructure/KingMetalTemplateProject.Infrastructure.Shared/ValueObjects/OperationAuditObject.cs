using Golden.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

namespace KingMetalTemplateProject.Infrastructure.Shared.ValueObjects;

/// <summary>
///     操作审计记录
/// </summary>
/// <param name="Operator">操作人</param>
/// <param name="OperationTime">操作时间</param>
/// <param name="Remark">备注</param>
/// <param name="BizType">操作的业务类型</param>
/// <param name="ActorId">ActorId</param>
/// <param name="ClientInfo">请求的客户端信息</param>
/// <param name="AttachData">附加数据</param>
/// <param name="SubjectId">操作子项Id, 例如操作某个子项的Id</param>
/// <param name="SubjectRemark">操作子项说明, 例如操作某个子项的说明</param>
/// <remarks>
///     <para> 张三 新增了 xxx</para>
///     <para> 张三 将 xxx 修改为  xxx</para>
///     <para> 张三 删除了  xxx</para>
/// </remarks>
public record OperationAuditObject(
    OperatorObject<string> Operator,
    long OperationTime,
    string? Remark,
    BizType BizType,
    long ActorId,
    RequestClientInfo? ClientInfo = null,
    object? AttachData = null,
    long SubjectId = 0,
    string SubjectRemark = ""
);