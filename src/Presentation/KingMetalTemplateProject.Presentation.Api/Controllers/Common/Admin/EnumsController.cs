﻿// -----------------------------------------------------------------------
//  <last-editor>黄深</last-editor>
//  <last-date>2022-05-30 17:26</last-date>
// -----------------------------------------------------------------------

using GoldCloud.Infrastructure.WebBase.Controllers.V1;
using KingMetalTemplateProject.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoldCloud.Presentation.PromotionApi.Controllers.Common.Admin;

/// <summary>
///     枚举接口
/// </summary>
[ControllerName(ApplicationConstants.EnumsController)]
[ApiExplorerSettings(GroupName = ApplicationConstants.AdminGroup)]
public class EnumsController : AdminApiBaseController
{
    #region 获取所有枚举

    /// <summary>
    ///     获取所有枚举
    /// </summary>
    /// <returns>
    /// </returns>
    [HttpGet]
    [ActionName("List")]
    [ProducesResponseType(typeof(List<object>), 200)]
    public IActionResult GetAccountStatusEnums([FromServices] ICommonApiService apiService)
    {
        return new OkObjectResult(apiService.GetEnums());
    }

    #endregion 获取所有枚举
}