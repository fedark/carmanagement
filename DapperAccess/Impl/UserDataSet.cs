using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class UserDataSet : IDataSet<User>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public UserDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<User?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<User>(cmd, new { Id = id })).SingleOrDefault();
    }
}
