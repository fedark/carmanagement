using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class UserRoleDataSet : IDataSet<UserRole>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public UserRoleDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<UserRole?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<UserRole>(cmd, new { Id = id })).SingleOrDefault();
    }
}
