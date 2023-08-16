using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

namespace Orleans;

/// <summary>
///     GrainFactory 扩展
/// </summary>
public static class GrainFactoryExtensions
{
    #region 公共方法

    /// <summary>
    ///     生成整形全局唯一ID
    /// </summary>
    /// <param name="grainFactory"></param>
    /// <returns></returns>
    public static async Task<long> NewIntegerIdAsync(this IGrainFactory grainFactory)
    {
        return await grainFactory.GetUniqueIdService().NewIntegerId();
    }

    /// <summary>
    ///     生成业务代码
    /// </summary>
    /// <param name="grainFactory"></param>
    /// <param name="orderType"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static async Task<string> NewBusinessNoAsync(this IGrainFactory grainFactory, BizType orderType,
        bool prefix = false)
    {
        var code = orderType.ToInt32().ToString().PadLeft(2, '0');
        return await grainFactory.GetUniqueIdService().NewStringId(code, prefix);
    }

    #endregion
}