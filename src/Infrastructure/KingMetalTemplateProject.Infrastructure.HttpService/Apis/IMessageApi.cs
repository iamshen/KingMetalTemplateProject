using GoldCloud.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.HttpService.Dtos.Message;
using Refit;

namespace KingMetalTemplateProject.Infrastructure.HttpService.Apis;

/// <summary>
///     消息系统
/// </summary>
internal interface IMessageApi
{
    /// <summary>
    ///     发送短信
    /// </summary>
    /// <returns></returns>
    [Post("/api/message/sms/sendNotice")]
    Task<ResponseResult<bool>> SendAsync([Body] SendSmsRequest request, [HeaderCollection] IDictionary<string, string>? headers = null);
}