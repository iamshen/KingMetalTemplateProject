namespace KingMetalTemplateProject.Infrastructure.HttpService.Dtos
{
    /// <summary>
    ///
    /// </summary>
    public record RpcResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public RpcResult(int code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; init; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => Code == 1;

        /// <summary>
        /// 返回成功.
        /// </summary>
        /// <returns></returns>
        public static RpcResult Ok(string message = "成功") => new(1, message);

        /// <summary>
        /// 返回成功.
        /// </summary>
        /// <returns></returns>
        public static RpcResult<TData> FromData<TData>(TData data, string message = "成功") => new(1, message, data);

        /// <summary>
        /// 返回失败.
        /// </summary>
        /// <returns></returns>
        public static RpcResult Fail(string message) => new(0, message);

        /// <summary>
        /// 处理失败.
        /// </summary>
        /// <returns></returns>
        public static RpcResult<TData> Fail<TData>(string message) => new(0, message);

        /// <summary>
        /// 处理失败.
        /// </summary>
        /// <returns>.</returns>
        public static RpcResult<TData> Fail<TData>(string message, TData data) => new(0, message, data);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public record RpcResult<TData> : RpcResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public RpcResult(int code, string message) : base(code, message)
        {
            Data = default;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public RpcResult(int code, string message, TData data) : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 业务数据
        /// </summary>
        public TData? Data { get; init; }
    }
}
