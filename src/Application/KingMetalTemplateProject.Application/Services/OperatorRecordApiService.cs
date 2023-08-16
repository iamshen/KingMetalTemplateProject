using GoldCloud.Infrastructure.Common.ValueObjects;
using KingMetalTemplateProject.Domain.Interfaces.Services;
using KingMetalTemplateProject.Infrastructure.Database;
using KingMetalTemplateProject.Infrastructure.Shared.Dtos.OperatorRecord;
using KingMetalTemplateProject.Infrastructure.Shared.Repository;
using LinqToDB;

namespace KingMetalTemplateProject.Application.Services;

/// <summary>
///     操作记录 Api 服务
/// </summary>
public class OperatorRecordApiService : BaseApiService, IOperatorRecordApiService
{
    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="provider"></param>
    public OperatorRecordApiService(IServiceProvider provider) : base(provider)
    {
    }

    /// <summary> 数据库查询仓储 </summary>
    public IQueryBaseRepository<GcTOperateRecord> Repository => GetRepository<GcTOperateRecord>();

    /// <summary>
    ///     分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PagedList<OperateRecordOutputDto>> GetListAsync(GetOperatorListDto input)
    {
        var response = new PagedList<OperateRecordOutputDto>();

        var query = Repository.Where()
                .WhereIf(x => x.ActorId == input.ActorId, input.ActorId != default)
                .WhereIf(x => x.SubjectId == input.SubjectId, input.SubjectId != default)
                .WhereIf(x => x.OperatorId == input.OperatorId, !input.OperatorId.IsNullOrWhiteSpace())
                .WhereIf(x => x.OperatorUserName == input.OperatorUserName,
                    !input.OperatorUserName.IsNullOrWhiteSpace())
                .WhereIf(x => x.OperatorRealName == input.OperatorRealName,
                    !input.OperatorRealName.IsNullOrWhiteSpace())
                .WhereIf(x => x.BizType == input.BizType!.Value, input.BizType.HasValue)
            ;

        var count = await query.CountAsync();

        var rows = await query.PageBy(input, x => x.Id).ToListAsync();
        if (rows.Any())
        {
            var dataList = Mapper.Map<List<OperateRecordOutputDto>>(rows);
            response.Data.AddRange(dataList);
        }

        response.TotalCount = count;

        return response;
    }
}