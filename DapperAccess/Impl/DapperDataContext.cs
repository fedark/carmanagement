using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class DapperDataContext : IDataContext
{
    private readonly SqlConnection connection_;

    public IDataSet<Car> Cars => new CarDataSet(connection_, "Cars");
    public IDataSet<Company> Companies => new CompanyDataSet(connection_, "Companies");
    public IDataSet<Model> Models => new ModelDataSet(connection_, "Models");

    public IDataSet<User> Users => new UserDataSet(connection_, "Users");
    public IDataSet<Role> Roles => new RoleDataSet(connection_, "Roles");
    public IDataSet<UserRole> UserRoles => new UserRoleDataSet(connection_, "UserRoles");

    public DapperDataContext(string connectionString)
    {
        connection_ = new SqlConnection(connectionString);
        connection_.Open();
    }

    public void Dispose()
    {
        connection_.Close();
    }
}
