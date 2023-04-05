using Data.Models;

namespace Data.Access.Abstractions;
public interface IDataContext : IDisposable
{
    IDataSet<Car> Cars { get; }
    IDataSet<Company> Companies { get; }
    IDataSet<Model> Models { get; }

    IDataSet<User> Users { get; }
    IDataSet<Role> Roles { get; }
    IDataSet<UserRole> UserRoles { get; }
}
