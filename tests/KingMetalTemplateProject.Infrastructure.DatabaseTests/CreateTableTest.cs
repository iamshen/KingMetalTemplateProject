using KingMetalTemplateProject.Infrastructure.Database;
using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using LinqToDB;

namespace KingMetalTemplateProject.Infrastructure.DatabaseTests;

/// <summary>
/// </summary>
public class CreateTableTest : IClassFixture<DependencySetupFixture>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="fixture"></param>
    public CreateTableTest(DependencySetupFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    #region 操作记录

    /// <summary>
    ///     操作记录
    /// </summary>
    [Fact(DisplayName = "1. 创建-操作记录-表")]
    public async Task CreateCompanyTableAsync()
    {
        //await using var db = _serviceProvider.GetAppDataConnection();
        //await db.DropTableAsync<GcTOperateRecord>(throwExceptionIfNotExists: false);
        //await db.CreateTableAsync<GcTOperateRecord>();
        await Task.Delay(1);
        Assert.True(true);
    }

    #endregion
}