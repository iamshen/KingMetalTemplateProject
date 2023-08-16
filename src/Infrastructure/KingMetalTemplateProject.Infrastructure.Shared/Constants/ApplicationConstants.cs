namespace Microsoft.AspNetCore.Mvc;

/// <summary>
///     应用常量
/// </summary>
public static class ApplicationConstants
{
    #region System

    /// <summary> StreamProviderName </summary>
    public const string StreamProviderName = "XStreamProvider";

    /// <summary> PubSubStore </summary>
    public const string MemoryGrainStorageName = "PubSubStore";

    /// <summary> Environment Prefix Name </summary>
    public const string EnvironmentPrefixName = "ASPNETCORE_";

    #endregion

    #region Controller Names

    /// <summary> 操作记录 </summary>
    public const string OperateRecordController = "OperateRecord";
    
    /// <summary>Enums </summary>
    public const string EnumsController = "Enums";

    #endregion Controller Names

    #region Api GroupNames

    /// <summary>admin </summary>
    public const string AdminGroup = "admin";

    /// <summary>device </summary>
    public const string DeviceGroup = "device";

    /// <summary>mch </summary>
    public const string MchGroup = "mch";

    /// <summary>mobile </summary>
    public const string MobileGroup = "mobile";

    /// <summary>open </summary>
    public const string OpenGroup = "open";

    /// <summary> default </summary>
    public const string DefaultGroup = "default";

    #endregion
}
