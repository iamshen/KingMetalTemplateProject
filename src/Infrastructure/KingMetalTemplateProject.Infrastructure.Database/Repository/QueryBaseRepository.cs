using System.Linq.Expressions;
using KingMetalTemplateProject.Infrastructure.Database.Extensions;
using KingMetalTemplateProject.Infrastructure.Shared.Repository;
using LinqToDB;

namespace KingMetalTemplateProject.Infrastructure.Database.Repository;

/// <summary>
///     数据库查询仓储
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class QueryBaseRepository<TEntity> : IQueryBaseRepository<TEntity> where TEntity : class, IEntity
{
    private readonly IServiceProvider _serviceProvider;

    public QueryBaseRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// </summary>
    /// <param name="predicate">数据过滤表达式</param>
    /// <returns>符合条件的数据集</returns>
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>>? predicate = null)
    {
        using var db = _serviceProvider.GetAppDataConnection();

        predicate ??= entity => entity.Id >= 0;

        return db.GetTable<TEntity>().Where(predicate);
    }

    /// <summary>
    ///     异步检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件谓语表达式</param>
    /// <returns>是否存在</returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await using var db = _serviceProvider.GetAppDataConnection();
        return await db.GetTable<TEntity>().AnyAsync(predicate);
    }

    /// <summary>
    ///     查找第一个符合条件的数据
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>符合条件的输出Dto，不存在时返回null</returns>
    public async Task<TEntity?> FirstOrDefaultAsync(long id)
    {
        await using var db = _serviceProvider.GetAppDataConnection();

        return await db.GetTable<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    ///     查找第一个符合条件的数据
    /// </summary>
    /// <param name="predicate">数据查询谓语表达式</param>
    /// <returns>符合条件的输出Dto，不存在时返回null</returns>
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        await using var db = _serviceProvider.GetAppDataConnection();

        return await db.GetTable<TEntity>().FirstOrDefaultAsync(predicate);
    }
}