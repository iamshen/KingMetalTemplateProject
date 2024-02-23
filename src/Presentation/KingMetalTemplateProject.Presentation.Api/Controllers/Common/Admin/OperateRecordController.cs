// -----------------------------------------------------------------------
//  <last-editor>黄深</last-editor>
//  <last-date>2022-09-16 15:36</last-date>
// -----------------------------------------------------------------------

using Golden.Infrastructure.Common.ValueObjects;
using Golden.Infrastructure.WebBase.Controllers.V1;
using KingMetalTemplateProject.Domain.Interfaces.Services;
using KingMetalTemplateProject.Infrastructure.Shared.Dtos.OperatorRecord;
using Microsoft.AspNetCore.Mvc;

namespace GoldCloud.Presentation.CrmApi.Controllers.Common.Admin;

/// <summary>
///     操作记录
/// </summary>
[ControllerName(ApplicationConstants.OperateRecordController)]
[ApiExplorerSettings(GroupName = ApplicationConstants.AdminGroup)]
public class OperateRecordController : AdminApiBaseController
{
    #region 分页查询操作记录

    /// <summary>
    ///     分页查询操作记录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="apiService"></param>
    /// <returns> </returns>
    [HttpGet]
    [ApiVersion("1.0")]
    [ActionName("List")]
    [ProducesResponseType(typeof(PagedList<OperateRecordOutputDto>), 200)]
    public async Task<IActionResult> GetVirtualWarehouseList([FromQuery] GetOperatorListDto request,
        [FromServices] IOperatorRecordApiService apiService)
    {
        var response = await apiService.GetListAsync(request);

        return new OkObjectResult(response);
    }

    #endregion 分页查询虚拟仓
}