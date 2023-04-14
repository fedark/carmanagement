using Data.Models;

namespace Data.Access.Abstractions;
public interface IRoleDataSet : IRoleDataSet<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
