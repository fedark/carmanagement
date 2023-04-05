using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class RoleDataSet : IDataSet<Role>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public RoleDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<Role?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<Role>(cmd, new { Id = id })).SingleOrDefault();
    }
}
