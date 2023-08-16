namespace System.Text.Json.Serialization;

#nullable disable

/// <summary>
///     动态类型Json序列化转换器
/// </summary>
/// <typeparam name="T"></typeparam>
public class DynamicJsonConverter<T> : JsonConverter<T>
{
    #region 是否能够转换

    /// <summary>
    ///     是否能够转换
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(T) == typeToConvert;
    }

    #endregion

    #region 获取具体动态类型

    /// <summary>
    ///     获取具体动态类型
    /// </summary>
    /// <param name="moduleValue"></param>
    protected virtual Type GetDynamicType(string moduleValue)
    {
        var type = Type.GetType(moduleValue);
        if (type is null)
        {
            throw new JsonException($"Failed to find the type with the specified discriminator value '{moduleValue}'");
        }

        return type;
    }

    #endregion

    #region 反序列化

    /// <summary>
    ///     反序列化
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Start object token type expected");
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty("Value", out var dataProperty))
        {
            throw new JsonException("Failed to find the required value property");
        }

        if (!jsonDocument.RootElement.TryGetProperty("Module", out var moduleProperty))
        {
            throw new JsonException("Failed to find the required module property");
        }

        var moduleValue = moduleProperty.GetString();
        if (string.IsNullOrEmpty(moduleValue) || string.IsNullOrWhiteSpace(moduleValue))
        {
            throw new JsonException("Failed to find the type");
        }

        var type = GetDynamicType(moduleValue);
        var valueJson = dataProperty.GetRawText();
        return (T)JsonSerializer.Deserialize(valueJson, type);
    }

    #endregion

    #region 序列化

    /// <summary>
    ///     序列化
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var vType = value.GetType();
        var newValue = new { Value = (object)value, Module = $"{vType.FullName},{vType.Assembly.GetName().Name}" };

        JsonSerializer.Serialize(writer, (object)newValue);
    }

    #endregion
}
