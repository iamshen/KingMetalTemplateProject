// -----------------------------------------------------------------------
//  <last-editor>黄深</last-editor>
//  <last-date>2023-01-31 16:19</last-date>
// -----------------------------------------------------------------------

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;

namespace KingMetalTemplateProject.Infrastructure.Database;

/// <summary>
///     DataBase Connection
/// </summary>
public class AppDataConnection : DataConnection
{
    #region 操作记录

    /// <summary>
    ///     操作记录
    /// </summary>
    public ITable<GcTOperateRecord> OperateRecords => this.GetTable<GcTOperateRecord>();

    #endregion 操作记录

    #region 初始化

    /// <summary>
    ///     初始化一个 <see cref="AppDataConnection" /> 类型的新实例
    /// </summary>
    /// <param name="options"> 配置对象 </param>
    public AppDataConnection(DataOptions options) : base(options)
    {
    }

    /// <summary>
    ///     初始化一个 <see cref="AppDataConnection" /> 类型的新实例
    /// </summary>
    /// <param name="connectionString"> 数据库连接字符串 </param>
    public AppDataConnection(string connectionString) : base(PostgreSQLTools.GetDataProvider(PostgreSQLVersion.v95),
        connectionString)
    {
    }

    #endregion 初始化
}