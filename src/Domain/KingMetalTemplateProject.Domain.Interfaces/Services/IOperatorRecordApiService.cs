using GoldCloud.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Infrastructure.Shared.Dtos.OperatorRecord;

namespace KingMetalTemplateProject.Domain.Interfaces.Services;

/// <summary>
///     操作记录Api 服务接口
/// </summary>
public interface IOperatorRecordApiService
{
    /// <summary>
    ///     分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<PagedList<OperateRecordOutputDto>> GetListAsync(GetOperatorListDto input);
}