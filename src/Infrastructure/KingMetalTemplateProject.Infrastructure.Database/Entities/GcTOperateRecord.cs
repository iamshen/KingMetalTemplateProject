using GoldCloud.Infrastructure.Common.ValueObjects;
using GoldCloud.Infrastructure.LinqToDb.Converter;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;
using KingMetalTemplateProject.Infrastructure.Shared.Repository;
using LinqToDB;
using LinqToDB.Mapping;

namespace KingMetalTemplateProject.Infrastructure.Database;

/// <summary>
///     操作记录
/// </summary>
[Table(Schema = TableConstants.DbSchema, Name = "gc_operate_record")]
public class GcTOperateRecord : IEntity
{
    /// <summary>
    ///     主键Id
    /// </summary>
    [Column("id", IsPrimaryKey = true, CanBeNull = false)]
    public long Id { get; set; }
    
    /// <summary>
    ///     操作人Id
    /// </summary>
    [Column("operator_id", CanBeNull = true)]
    public string? OperatorId { get; set; }

    /// <summary>
    ///     操作人姓名
    /// </summary>
    [Column("operator_user_name", CanBeNull = true)]
    public string? OperatorUserName { get; set; }

    /// <summary>
    ///     操作人姓名
    /// </summary>
    [Column("operator_real_name", CanBeNull = true)]
    public string? OperatorRealName { get; set; }

    /// <summary>
    ///     操作业务类型
    /// </summary>
    [Column("biz_type", DataType = DataType.Int16, CanBeNull = false)]
    public BizType BizType { get; set; }

    /// <summary>
    ///     ActorId
    /// </summary>
    [Column("actorId", CanBeNull = true)]
    public long ActorId { get; set; }

    /// <summary>
    ///     请求客户端信息
    /// </summary>
    [Column("request_client_info", DataType = DataType.BinaryJson, CanBeNull = true)]
    [ValueConverter(ConverterType = typeof(JsonValueConverter<RequestClientInfo>))]
    public RequestClientInfo? RequestClientInfo { get; set; }

    /// <summary>
    ///     附加数据
    /// </summary>
    [Column("attach_data", DataType = DataType.BinaryJson, CanBeNull = true)]
    [ValueConverter(ConverterType = typeof(JsonValueConverter<object>))]
    public object? AttachData { get; set; }

    /// <summary>
    ///     操作子项Id, 例如操作某个子项的Id
    /// </summary>
    [Column("subject_id", CanBeNull = true)]
    public long SubjectId { get; set; }

    /// <summary>
    ///     操作子项说明, 例如操作某个子项的说明
    /// </summary>
    [Column("subject_remark", CanBeNull = true)]
    public string? SubjectRemark { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    [Column("remark", CanBeNull = true)]
    public string? Remark { get; set; }

    /// <summary>
    ///     操作时间
    /// </summary>
    [Column("operated_time", CanBeNull = false)]
    public long OperationTime { get; set; }

}