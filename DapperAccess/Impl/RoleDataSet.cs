using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class RoleDataSet : IDataSet<Role>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;
    private readonly string userRoleTable_;
    private readonly string userTable_;

    public RoleDataSet(SqlConnection connection, string tableName, string userRoleTable, string userTable)
    {
        connection_ = connection;
        tableName_ = tableName;
        userRoleTable_ = userRoleTable;
        userTable_ = userTable;
    }

    public async Task AddAsync(Role entity)
    {
        var cmd = $"insert into {tableName_} values (@id, @name)";
        await connection_.ExecuteAsync(cmd, entity);

        var userCmd = $"insert into {userRoleTable_} values (@userId, @roleId)";
        await connection_.ExecuteAsync(userCmd, entity.Users.Select(u => new { UserId = u.Id, RoleId = entity.Id }));
    }

    public Task DeleteAsync(string id)
    {
        var cmd = $"delete from {tableName_} where {nameof(Role.Id)} = @id";
        return connection_.ExecuteAsync(cmd, new { id });
    }

    public Task<IEnumerable<Role>> GetAllAsync()
    {
        var cmd = $@"select r.*, u.* from {tableName_} r
                        left join {userRoleTable_} ur on r.{nameof(Role.Id)} = ur.RoleId
                        left join {userTable_} u on ur.UserId = u.{nameof(User.Id)}";
        return connection_.QueryAsync<Role, User?, Role>(cmd, (role, user) =>
        {
            if (user is not null)
            {
                role.Users.Add(user);
            }

            return role;
        });
    }

    public async Task<Role?> GetAsync(string id)
    {
        var cmd = $@"select r.*, u.* from {tableName_} r
                        left join {userRoleTable_} ur on r.{nameof(Role.Id)} = ur.RoleId
                        left join {userTable_} u on ur.UserId = u.{nameof(User.Id)}
                        where @r.{nameof(Role.Id)} = @id";
        return (await connection_.QueryAsync<Role, User?, Role>(cmd, (role, user) =>
        {
            if (user is not null)
            {
                role.Users.Add(user);
            }

            return role;
        },
        new { id })).SingleOrDefault();
    }

    public Task<IEnumerable<Role>> GetRangeAsync(int from, int to)
    {
        ThrowHelper.ValidateRange(from, to);

        var cmd = $@"select r.*, u.* from {tableName_} r
                        left join {userRoleTable_} ur on r.{nameof(Role.Id)} = ur.RoleId
                        left join {userTable_} u on ur.UserId = u.{nameof(User.Id)}
                        order by r.{nameof(Role.Id)}
                        offset @offset rows
                        fetch next @fetch rows only";
        return connection_.QueryAsync<Role, User?, Role>(cmd, (role, user) =>
        {
            if (user is not null)
            {
                role.Users.Add(user);
            }

            return role;
        },
        new { Offset = from - 1, Fetch = to - from + 1 });
    }

    public async Task UpdateAsync(Role entity)
    {
        var cmd = @$"update {tableName_} 
                    set {nameof(Role.Name)} = @name
                    where {nameof(Role.Id)} = @id";
        await connection_.ExecuteAsync(cmd, entity);

        var userCmd = $"select UserId from {userRoleTable_} where RoleId = @roleId";
        var oldUsers = await connection_.QueryAsync<string>(userCmd, new { roleId = entity.Id });

        var newUsers = entity.Users.Select(u => u.Id);

        userCmd = $"delete from {userRoleTable_} where UserId in (@userIds) and RoleId = @roleId";
        await connection_.ExecuteAsync(userCmd, new { UserIds = oldUsers.Except(newUsers), RoleIds = entity.Id });

        userCmd = $"insert into {userRoleTable_} values (@userId, @roleId)";
        await connection_.ExecuteAsync(userCmd, newUsers.Except(oldUsers).Select(u => new { UserId = u, RoleId = entity.Id }));
    }
}
