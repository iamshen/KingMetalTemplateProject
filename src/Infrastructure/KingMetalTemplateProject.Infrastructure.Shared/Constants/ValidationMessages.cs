using KingMetalTemplateProject.Infrastructure.Shared.Cultures.Commands;

namespace System.ComponentModel.DataAnnotations;

/// <summary>
///     参数验证异常信息
/// </summary>
public static class ValidationMessages
{
    /// <summary>  参数 {0} 的值无效 </summary>
    public const string Invalid = "参数 {0} 的值无效";

    /// <summary>  参数 {0} 的值 对 {1} 无效 </summary>
    public const string InvalidEnum = "参数 {0} 的值 对 {1} 无效";

    /// <summary>  参数{0}验证失败,字符长度不能大于{1} </summary>
    public const string MaxLength = "参数{0}验证失败,字符长度不能大于{1}";

    /// <summary>  参数{0}验证失败,字符长度不能小于{1} </summary>
    public const string MinLength = "参数{0}验证失败,字符长度不能小于{1}";

    /// <summary>  参数{0}验证失败,值必须在{1}到{2}之间 </summary>
    public const string Range = "参数{0}验证失败,值必须在{1}到{2}之间";

    /// <summary>  参数 {0} 不能为空 </summary>
    public const string Required = "参数 {0} 不能为空";

    /// <summary>  无效的手机号码 </summary>
    public const string InvalidMobile = "无效的手机号码";

    /// <summary>  StringLength </summary>
    public const string StringLength = "参数{0}验证失败,字符长度必须在{2}到{1}之间";

    /// <summary>   Url </summary>
    public const string Url = "参数{0}验证失败,不是有效的URL地址";

    /// <summary> 操作没有引发任何变化 </summary>
    public const string NotChanged = "操作没有引发任何变化";

    /// <summary> 操作没有引发任何变化 </summary>
    public const string InValidUserNameOrPwd = "无效的用户名或密码";

    /// <summary>   配置 {0} 已存在，请勿重复添加 </summary>
    public const string AlreadyExistConfig = "配置 {0} 已存在，请勿重复添加";

    /// <summary> 找不到配置项 {0}。</summary>
    public const string ConfigNotFound = "找不到配置项 {0}。";

    /// <summary>  找不到配置 {0}, 请先添加配置。</summary>
    public const string EmptyConfigNotFound = "找不到配置 {0}, 请先添加配置。";

    #region Operation

    /// <summary>
    ///     获取命令的对应语言的描述信息
    /// </summary>
    /// <param name="commandName"> </param>
    /// <returns> </returns>
    public static string? GetCommandRemark(string commandName)
    {
        return Language.ResourceManager.GetString(commandName);
    }

    #endregion
}