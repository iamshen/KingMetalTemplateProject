namespace KingMetalTemplateProject.Infrastructure.HttpService.Dtos.Message;

/// <summary>
///     发送短信
/// </summary>
internal class SendSmsRequest
{
    /// <summary>
    ///     手机号码
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    ///     短信模板ID
    /// </summary>
    public string TemplateCode { get; set; } = string.Empty;

    /// <summary>
    ///     短信模板参数
    /// </summary>
    public IDictionary<string, string> TemplateParam { get; set; } = new Dictionary<string, string>();
}