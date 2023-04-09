using System.Data;
using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class UserDataSet : IDataSet<User>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;
    private readonly string userRoleTable_;
    private readonly string roleTable_;

    public UserDataSet(SqlConnection connection, string tableName, string userRoleTable, string roleTable)
    {
        connection_ = connection;
        tableName_ = tableName;
        userRoleTable_ = userRoleTable;
        roleTable_ = roleTable;
    }

    public async Task AddAsync(User entity)
    {
        var cmd = $"insert into {tableName_} values (@id, @name, @passwordHash, @hasDriverLicense)";
        await connection_.ExecuteAsync(cmd, entity);

        var roleCmd = $"insert into {userRoleTable_} values (@userId, @roleId)";
        await connection_.ExecuteAsync(roleCmd, entity.Roles.Select(r => new { UserId = entity.Id, RoleId = r.Id }));
    }

    public Task DeleteAsync(string id)
    {
        var cmd = $"delete from {tableName_} where {nameof(User.Id)} = @id";
        return connection_.ExecuteAsync(cmd, new { id });
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        var cmd = $@"select u.*, r.* from {tableName_} u
                        join {userRoleTable_} ur on u.{nameof(User.Id)} = ur.UserId
                        join {roleTable_} r on ur.RoleId = r.{nameof(Role.Id)}";
        return connection_.QueryAsync<User, Role, User>(cmd, (user, role) =>
        {
            user.Roles.Add(role);
            return user;
        });
    }

    public async Task<User?> GetAsync(string id)
    {
        var cmd = $@"select u.*, r.* from {tableName_}
                        join {userRoleTable_} ur on u.{nameof(User.Id)} = ur.UserId
                        join {roleTable_} r on ur.RoleId = r.{nameof(Role.Id)}
                        where u.{nameof(User.Id)} = @id";
        return (await connection_.QueryAsync<User, Role, User>(cmd, (user, role) =>
        {
            user.Roles.Add(role);
            return user;
        },
        new { id })).SingleOrDefault();
    }

    public async Task UpdateAsync(User entity)
    {
        var cmd = @$"update {tableName_} 
                    set {nameof(User.Name)} = @name,
                        {nameof(User.PasswordHash)} = @passwordHash,
                        {nameof(User.HasDriverLicense)} = @hasDriverLicense
                    where {nameof(User.Id)} = @id";
        await connection_.ExecuteAsync(cmd, entity);

        var roleCmd = $"select RoleId from {userRoleTable_} where UserId = @userId";
        var oldRoles = await connection_.QueryAsync<string>(roleCmd, new { userId = entity.Id });

        var newRoles = entity.Roles.Select(r => r.Id);

        roleCmd = $"delete from {userRoleTable_} where UserId = @userId and RoleId in (@roleIds)";
        await connection_.ExecuteAsync(roleCmd, new { UserId = entity.Id, RoleIds = oldRoles.Except(newRoles) });

        roleCmd = $"insert into {userRoleTable_} values (@userId, @roleId)";
        await connection_.ExecuteAsync(roleCmd, newRoles.Except(oldRoles).Select(r => new { UserId = entity.Id, RoleId = r }));
    }
}
