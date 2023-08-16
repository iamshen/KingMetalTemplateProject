namespace KingMetalTemplateProject.Infrastructure.Shared.Repository;

/// <summary>
///     IEntity
/// </summary>
public interface IEntity
{
    /// <summary>
    ///     主键
    /// </summary>
    public long Id { get; set; }
}