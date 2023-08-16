using GoldCloud.Infrastructure.Common.ValueObjects;
using KingMetal.Domains.Abstractions.Command;

namespace Orleans;

public static class KingMetalOperatorCommandExtensions
{
    /// <summary>
    ///     命令对象转换为操作对象
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static OperatorObject<T> GetOperatorObject<T>(this KingMetalOperatorCommand command) where T : IEquatable<T>
    {
        return new OperatorObject<T>
        {
            OperatorId = command.OperatorId.CastTo<T>(),
            OperatorUserName = command.OperatorUserName,
            OperatorRealName = command.OperatorRealName
        };
    }

    /// <summary>
    ///     获取请求的客户端信息
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static RequestClientInfo? GetClientInfo(this KingMetalOperatorCommand command)
    {
        return command.ClientInfo == null
            ? new RequestClientInfo
            {
                Ip = command.ClientInfo?.Ip,
                UserAgent = command.ClientInfo?.UserAgent,
                HostInfo = command.ClientInfo?.HostInfo
            }
            : null;
    }
}