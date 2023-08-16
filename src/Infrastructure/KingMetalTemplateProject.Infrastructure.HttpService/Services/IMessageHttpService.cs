using KingMetalTemplateProject.Infrastructure.HttpService.Dtos;

namespace KingMetalTemplateProject.Infrastructure.HttpService.Services;

/// <summary>
///     消息服务
/// </summary>
public interface IMessageHttpService
{
    /// <summary>
    ///     付款成功通知
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <remarks>SMS_209830975: 您的订单${OrderNo}已付款成功，请留意您${text}的入账信息，非常感谢您的支持.</remarks>
    Task<RpcResult> SmsPaymentAsync(object input);
}