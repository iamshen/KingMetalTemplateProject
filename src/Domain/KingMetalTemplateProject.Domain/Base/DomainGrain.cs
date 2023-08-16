using System.ComponentModel.DataAnnotations;
using GoldCloud.Infrastructure.Common.Exception;
using KingMetal.Domains.Abstractions.Command;
using KingMetal.Domains.Abstractions.Exceptions;
using KingMetal.Domains.Abstractions.State;
using KingMetal.Domains.Core.Grains;
using KingMetal.Infrastructures.ObjectType.Common;
using KingMetalTemplateProject.Domain.Interfaces.Events.Common;
using KingMetalTemplateProject.Infrastructure.Shared.Enumerations;
using KingMetalTemplateProject.Infrastructure.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using Orleans;

namespace KingMetalTemplateProject.Domain.Grain;

/// <summary>
///     domain Grain base implement
/// </summary>
/// <typeparam name="TState"></typeparam>
public abstract class DomainGrain<TState> : DomainGrain<TState, long> where TState : class, IState<TState>, new()
{
    /// <summary>
    ///     grain 名称
    /// </summary>
    protected abstract string GrainCnName { get; }

    #region 生成操作记录事件

    /// <summary>
    ///     生成操作记录事件
    /// </summary>
    /// <param name="command"></param>
    /// <param name="bizType"></param>
    /// <param name="remark"></param>
    /// <param name="attachData"></param>
    /// <param name="subjectId"></param>
    /// <param name="subjectRemark"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual OperationRecordAddedEvent NewOperationRecordEvent(
        KingMetalOperatorCommand command,
        BizType bizType,
        string? remark = "",
        object? attachData = null,
        long subjectId = 0,
        string subjectRemark = "",
        params object[] args)
    {
        if (remark.IsNullOrWhiteSpace())
            try
            {
                var desc = ValidationMessages.GetCommandRemark(command.CommandName);
                remark = args.Any() && !string.IsNullOrWhiteSpace(desc)
                    ? string.Format(desc, args)
                    : ValidationMessages.GetCommandRemark(command.CommandName);
            }
            catch
            {
                Logger.LogWarning("命令 [{Command}] 格式化审计日志异常", command.CommandName);
            }


        return new OperationRecordAddedEvent
        {
            OperationAudit = new OperationAuditObject(
                command.GetOperatorObject<string>(),
                MetalDateTime.Now,
                remark,
                bizType,
                ActorId,
                command.GetClientInfo(),
                attachData,
                subjectId,
                subjectRemark)
        };
    }

    #endregion

    #region 检查Grain版本号

    /// <summary>
    ///     检查Grain的版本号是否小于等于0,如果是则抛出异常
    /// </summary>
    /// <exception cref="InvalidArgumentException"></exception>
    protected override void ThrowUninitialized()
    {
        base.ThrowUninitialized();
        if (State.StateMetadata.Version <= 0L) throw new InvalidArgumentException(string.Concat("无效的", GrainCnName));
    }

    /// <summary>检查Grain的版本号是否大于0,如果大于0则抛出异常</summary>
    protected override void ThrowInitialized()
    {
        if (State.StateMetadata.Version > 0L)
            throw new AlreadyExistedException(string.Concat(GrainCnName, "已存在，请勿重复操作"));
    }

    #endregion
}