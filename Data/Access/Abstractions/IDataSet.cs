namespace Data.Access.Abstractions;
public interface IDataSet<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(string id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetRangeAsync(int from, int to);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(string id);
}
