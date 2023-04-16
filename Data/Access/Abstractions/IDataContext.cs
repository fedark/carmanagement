using Data.Models;

namespace Data.Access.Abstractions;
public interface IDataContext : IDisposable
{
    IDataSet<Car> Cars { get; }
    IDataSet<Company> Companies { get; }
    IDataSet<Model> Models { get; }

    IUserDataSet Users { get; }
    IRoleDataSet Roles { get; }
}
