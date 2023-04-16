using Data.Models;

namespace Data.Access.Abstractions;
public interface IRoleDataSet : IDataSet<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
