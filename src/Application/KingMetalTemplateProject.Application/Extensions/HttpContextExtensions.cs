using System.Runtime.CompilerServices;
using System.Security.Claims;
using Golden.Infrastructure.Common.Exception;
using Golden.Infrastructure.Common.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.Extensions.DependencyInjection.Extensions;

#region HttpContext扩展方法

/// <summary>
///     HttpContext扩展方法
/// </summary>
public static class HttpContextExtensions
{
    #region 获取客户端的UserAgent

    /// <summary>
    ///     获取客户端的UserAgent
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValues GetRequestUserAgent(this HttpContext httpContext)
    {
        return httpContext.GetRequestHeaderValue("User-Agent");
    }

    #endregion

    #region 获取客户端的主机信息

    /// <summary>
    ///     获取客户端的UserAgent
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValues GetRequestHostInfo(this HttpContext httpContext)
    {
        return httpContext.GetRequestHeaderValue("Client_Os_Info");
    }

    #endregion

    #region 获取客户端请求头中的信息

    /// <summary>
    ///     获取客户端请求头中的信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringValues GetRequestHeaderValue(this HttpContext httpContext, string key)
    {
        if (httpContext.Request.Headers.TryGetValue(key, out var value)) return value;

        return "";
    }

    #endregion

    #region 获取请求的客户端信息

    /// <summary>
    ///     获取请求的客户端信息
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static RequestClientInfo GetClientInfo(this HttpContext httpContext)
    {
        return new RequestClientInfo(httpContext.GetRequestIp(), httpContext.GetRequestUserAgent(),
            httpContext.GetRequestHostInfo());
    }

    #endregion

    #region 获取客户端IP

    /// <summary>
    ///     获取客户端IP(如果有设置代理，则会尝试获取每一层代理转发的IP)
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetRequestIp(this HttpContext httpContext)
    {
        if (!string.IsNullOrEmpty(httpContext.GetRequestHeaderValue("X-Forwarded-For")))
            return httpContext.GetRequestHeaderValue("X-Forwarded-For").ToString();

        if (!string.IsNullOrEmpty(httpContext.GetRequestHeaderValue("X-Real-IP")))
            return httpContext.GetRequestHeaderValue("X-Real-IP").ToString();

        return httpContext.Connection.RemoteIpAddress.ToString();
    }

    /// <summary>
    ///     获取请求的客户端的IP(只获取请求客户端的IP)
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetRequestRemoteIp(this HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress.ToString();
    }

    #endregion

    #region 获取请求操作人对象

    public const string ClaimUserIdKey = "UserId";
    public const string ClaimRealNameKey = "RealName";
    public const string ClaimUserNameKey = "UserName";
    public const string ClaimClientKey = "client_id";

    public static OperatorObject<long> GetOperatorObject(this HttpContext httpContext)
    {
        var identity = httpContext.User.Identity;

        if (identity == null) throw new UnauthorizedException("未登录");

        if (identity is not ClaimsIdentity claimsIdentity) throw new UnauthorizedException("未登录");

        var operatorId = string.Empty;
        var operatorRealName = string.Empty;
        var operatorUserName = string.Empty;
        if (!string.IsNullOrWhiteSpace(claimsIdentity.FindFirst(ClaimUserIdKey)?.Value))
        {
            operatorId = claimsIdentity.FindFirst(ClaimUserIdKey)?.Value;
            operatorRealName = claimsIdentity.FindFirst(ClaimRealNameKey)?.Value;
            operatorUserName = claimsIdentity.FindFirst(ClaimUserNameKey)?.Value;
        }
        // 客户端模式
        else if (!string.IsNullOrWhiteSpace(claimsIdentity.FindFirst(ClaimClientKey)?.Value))
        {
            operatorId = claimsIdentity.FindFirst(ClaimClientKey)?.Value;
            operatorRealName = claimsIdentity.FindFirst(ClaimClientKey)?.Value;
            operatorUserName = claimsIdentity.FindFirst(ClaimClientKey)?.Value;
        }

        return new OperatorObject<long>
        {
            OperatorId = operatorId.CastTo<long>(0),
            OperatorRealName = operatorRealName,
            OperatorUserName = operatorUserName
        };
    }

    #endregion
}

#endregion