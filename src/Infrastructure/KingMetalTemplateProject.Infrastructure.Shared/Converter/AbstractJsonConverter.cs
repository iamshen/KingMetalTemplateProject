namespace System.Text.Json.Serialization;

/// <summary>
///     抽象类型Json序列化转换器
/// </summary>
/// <typeparam name="T"></typeparam>
public class AbstractJsonConverter<T> : DynamicJsonConverter<T>
{
    /// <summary>
    ///     抽象类型Json序列化转换器
    /// </summary>
    static AbstractJsonConverter()
    {
        var type = typeof(T);
        Types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p is { IsClass: true, IsAbstract: false })
            .ToList();
    }

    /// <summary>
    ///     抽象类型Json序列化转换器
    /// </summary>
    public AbstractJsonConverter()
    {
    }

    /// <summary> Abstract Types </summary>
    private static IEnumerable<Type> Types { get; }


    #region 是否能够转换

    /// <summary>
    ///     是否能够转换
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return Types.Any(typeToConvert.IsAssignableFrom);
    }

    #endregion

    #region 获取具体动态类型

    /// <summary>
    ///     获取具体动态类型
    /// </summary>
    /// <param name="moduleValue"></param>
    protected override Type GetDynamicType(string moduleValue)
    {
        var type = Types.FirstOrDefault(x => $"{x.FullName},{x.Assembly.GetName().Name}" == moduleValue);
        if (type is null)
            throw new JsonException($"Failed to find the type with the specified discriminator value '{moduleValue}'");

        return type;
    }

    #endregion
}