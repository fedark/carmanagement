using Dapper;
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

    public async Task<User?> GetUserByNameAsync(string name)
    {
        var cmd = $@"select u.*, r.* from {UserTable} u
                        left join {UserRoleTable} ur on u.{nameof(User.Id)} = ur.UserId
                        left join {RoleTable} r on ur.RoleId = r.{nameof(Role.Id)}
                        where u.{nameof(User.Name)} = @name";
        return (await connection_.QueryAsync<User, Role?, User>(cmd, (user, role) =>
        {
            if (role is not null)
            {
                user.Roles.Add(role);
            }

            return user;
        },
        new { name })).SingleOrDefault();
    }
}
