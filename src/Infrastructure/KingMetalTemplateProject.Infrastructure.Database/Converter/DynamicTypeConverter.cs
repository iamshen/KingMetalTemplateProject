using System.Text.Json;
using System.Text.Json.Serialization;

#nullable disable

namespace LinqToDB.Common
{
    #region 动态类型JSON和字符串的转换器

    /// <summary>
    /// 动态类型JSON和字符串的转换器
    /// </summary>
    public class DynamicTypeConverter<T> : ValueConverter<T, string>
    {
        #region 私有变量

        /// <summary>
        /// 序列化配置
        /// </summary>
        private static readonly JsonSerializerOptions _defaultSerializeOptions ;

        /// <summary>
        /// 反序列化配置
        /// </summary>
        private static readonly JsonSerializerOptions _defaultDeserializeOptions;

        /// <summary>
        /// 转换器
        /// </summary>
        private static readonly DynamicJsonConverter<T> _defaultConverter;

        #endregion

        #region 初始化

        /// <summary>
        /// JSON和字符串的转换器
        /// </summary>
        static DynamicTypeConverter()
        {
            _defaultConverter = new DynamicJsonConverter<T>();

            _defaultSerializeOptions = new();
            _defaultSerializeOptions.Converters.Add(_defaultConverter);
            _defaultDeserializeOptions = new();
            _defaultDeserializeOptions.Converters.Add(_defaultConverter);
        }

        /// <summary>
        /// JSON和字符串的转换器
        /// </summary>
        public DynamicTypeConverter() : this(default, default) { }

        /// <summary>
        /// JSON和字符串的转换器
        /// </summary>
        public DynamicTypeConverter(JsonSerializerOptions serializeOptions = default, JsonSerializerOptions deserializeOptions = default) :
            base(v => v == null ? null : Serialize(v, serializeOptions),
                 v => v == null ? default : Deserialize(v, deserializeOptions),
                 true)
        { }

        #endregion

        #region 序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static string Serialize(T value, JsonSerializerOptions options)
        {
            if (options is null) 
                return JsonSerializer.Serialize(value, _defaultSerializeOptions);
            else
            {
                options.Converters.Add(_defaultConverter);
                var json = JsonSerializer.Serialize(value, options);
                options.Converters.Remove(_defaultConverter);

                return json;
            }
        }

        #endregion

        #region 反序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static T Deserialize(string json, JsonSerializerOptions options)
        {
            if (options is null)
                return JsonSerializer.Deserialize<T>(json, _defaultDeserializeOptions);
            else
            {
                options.Converters.Add(_defaultConverter);
                var value = JsonSerializer.Deserialize<T>(json, options);
                options.Converters.Remove(_defaultConverter);

                return value;
            }
        }

        #endregion
    }

    #endregion
}
