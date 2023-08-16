using System.Linq.Expressions;

namespace KingMetalTemplateProject.Infrastructure.Shared.Repository;

/// <summary>
///     数据库查询仓储接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IQueryBaseRepository<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    ///     查询数据源
    /// </summary>
    /// <param name="predicate">数据过滤表达式</param>
    /// <returns>符合条件的数据集</returns>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>>? predicate = null);

    /// <summary>
    ///     异步检查实体是否存在
    /// </summary>
    /// <param name="predicate">查询条件谓语表达式</param>
    /// <returns>是否存在</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    ///     异步查找指定主键的实体
    /// </summary>
    /// <param name="id">实体主键</param>
    /// <returns>符合主键的实体，不存在时返回null</returns>
    Task<TEntity?> FirstOrDefaultAsync(long id);

    /// <summary>
    ///     查找第一个符合条件的数据
    /// </summary>
    /// <param name="predicate">数据查询谓语表达式</param>
    /// <returns>符合条件的输出Dto，不存在时返回null</returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
}