namespace Data.Access.Abstractions;
public interface IDataSet<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(string id);
}
