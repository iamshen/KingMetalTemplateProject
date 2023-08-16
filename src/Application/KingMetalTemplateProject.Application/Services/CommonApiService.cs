using GoldCloud.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Domain.Interfaces.Services;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

namespace KingMetalTemplateProject.Application.Services;

/// <summary> 公共 Api 服务 </summary>
public class CommonApiService : BaseApiService, ICommonApiService
{
    #region 初始化

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="provider"></param>
    public CommonApiService(IServiceProvider provider) : base(provider)
    {
    }

    #endregion

    /// <summary>
    ///     获取所有枚举
    /// </summary>
    /// <returns></returns>
    public async Task<List<object>> GetEnums()
    {
        return await Task.FromResult(GetEnumSubjects().ToList());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private IEnumerable<object> GetEnumSubjects()
    {
        yield return new EnumSubject<BizType>(nameof(BizType), "业务类型");
    }
}