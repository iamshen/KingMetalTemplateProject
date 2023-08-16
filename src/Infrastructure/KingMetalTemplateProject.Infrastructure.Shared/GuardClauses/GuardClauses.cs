namespace Ardalis.GuardClauses;

public static class BooleanGuard
{
    /// <summary>
    ///     如果 input 为 True 则抛出异常
    /// </summary>
    /// <param name="guardClause"></param>
    /// <param name="input"></param>
    /// <param name="parameterName"></param>
    /// <param name="message"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void True(this IGuardClause guardClause, bool input, string? parameterName = null, string? message = null)
    {
        if (input) throw new ArgumentException(message ?? "参数不符合要求", parameterName);
    }
}