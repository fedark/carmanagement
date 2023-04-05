using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class CompanyDataSet : IDataSet<Company>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public CompanyDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<Company?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<Company>(cmd, new { Id = id })).SingleOrDefault();
    }
}
