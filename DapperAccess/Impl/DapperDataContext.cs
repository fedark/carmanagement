using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class DapperDataContext : IDataContext
{
    private readonly SqlConnection connection_;

    private readonly string CarTable = "Cars";
    private readonly string CompanyTable = "Companies";
    private readonly string ModelTable = "Models";

    private readonly string UserTable = "Users";
    private readonly string RoleTable = "Roles";
    private readonly string UserRoleTable = "UserRoles";

    public IDataSet<Car> Cars => new CarDataSet(connection_, CarTable, ModelTable, CompanyTable);
    public IDataSet<Company> Companies => new CompanyDataSet(connection_, CompanyTable);
    public IDataSet<Model> Models => new ModelDataSet(connection_, ModelTable, CompanyTable);

    public IDataSet<User> Users => new UserDataSet(connection_, UserTable, UserRoleTable, RoleTable);
    public IDataSet<Role> Roles => new RoleDataSet(connection_, RoleTable, UserRoleTable, UserTable);

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
