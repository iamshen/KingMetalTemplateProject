namespace KingMetalTemplateProject.Infrastructure.Shared.ValueObjects;

/// <summary>
///     新订单对象
/// </summary>
/// <param name="OrderId"></param>
/// <param name="OrderNo"></param>
public record CreatedOrderObject(string OrderId, string OrderNo);