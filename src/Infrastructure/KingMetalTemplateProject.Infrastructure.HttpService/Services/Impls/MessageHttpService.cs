using GoldCloud.Infrastructure.Common.Enumerations;
using KingMetalTemplateProject.Infrastructure.HttpService.Apis;
using KingMetalTemplateProject.Infrastructure.HttpService.Dtos;
using KingMetalTemplateProject.Infrastructure.HttpService.Dtos.Message;
using Microsoft.Extensions.Logging;

namespace KingMetalTemplateProject.Infrastructure.HttpService.Services;

/// <summary>
///     消息 Http 服务
/// </summary>
internal class MessageHttpService : IMessageHttpService
{
    private const string SmsFail = "短信发送失败";
    private const string SmsError = "短信发送异常";


    /// <summary> </summary>
    private readonly IMessageApi _api;

    /// <summary> logger  </summary>
    private readonly ILogger _logger;

    /// <summary>
    ///     ctor
    /// </summary>
    public MessageHttpService(
        IMessageApi api,
        ILogger<MessageHttpService> logger)
    {
        _api = api;
        _logger = logger;
    }

    #region 付款成功通知

    /// <summary>
    ///     付款成功通知
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<RpcResult> SmsPaymentAsync(object input)
    {
        try
        {
            var response = await _api.SendAsync(new SendSmsRequest
            {
                // PhoneNumber = input.Phone,
                // TemplateCode = "SMS_209830975",
                // TemplateParam = new Dictionary<string, string>()
                // {
                //     { "OrderNo", input.OrderNo },
                //     { "text", $"尾号为{input.Account[^4..]}的{input.PayWay}" }
                // }
            });

            return response.ErrorCode is ErrorCode.Success ? RpcResult.Ok() : RpcResult.Fail(SmsFail);
        }
        catch (Exception exc)
        {
            _logger.LogError(exc, nameof(SmsPaymentAsync));

            return RpcResult.Fail(SmsError);
        }
    }

    #endregion 付款成功通知
}