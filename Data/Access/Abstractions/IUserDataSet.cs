using Data.Models;

namespace Data.Access.Abstractions;
public interface IUserDataSet : IDataSet<User>
{
    Task<User?> GetByNameAsync(string name);
}
