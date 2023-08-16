namespace KingMetalTemplateProject.Domain.Interfaces.Services;

/// <summary>
///     公共 api 服务接口
/// </summary>
public interface ICommonApiService
{
    /// <summary>
    ///     获取所有枚举
    /// </summary>
    /// <returns></returns>
    public Task<List<object>> GetEnums();
}