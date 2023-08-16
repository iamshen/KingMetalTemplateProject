using System.ComponentModel.DataAnnotations;
using Resources = KingMetalTemplateProject.Infrastructure.Shared.Cultures.BizType;

namespace KingMetalTemplateProject.Infrastructure.Shared.Enumerations;

/// <summary>
///     业务类型
/// </summary>
public enum BizType
{
    /// <summary>
    ///     默认
    /// </summary>
    [Display(Name = nameof(Default), ResourceType = typeof(Resources.Language))]
    Default = 1
}