using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class CarDataSet : IDataSet<Car>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public CarDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<Car?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<Car>(cmd, new { Id = id })).SingleOrDefault();
    }
}
