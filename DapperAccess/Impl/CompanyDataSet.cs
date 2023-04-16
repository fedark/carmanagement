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

    public Task AddAsync(Company entity)
    {
        var cmd = $"insert into {tableName_} values (@id, @name)";
        return connection_.ExecuteAsync(cmd, entity);
    }

    public Task DeleteAsync(string id)
    {
        var cmd = $"delete from {tableName_} where {nameof(Company.Id)} = @id";
        return connection_.ExecuteAsync(cmd, new { id });
    }

    public Task<IEnumerable<Company>> GetAllAsync()
    {
        var cmd = $"select * from {tableName_}";
        return connection_.QueryAsync<Company>(cmd);
    }

    public async Task<Company?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where {nameof(Company.Id)} = @id";
        return (await connection_.QueryAsync<Company>(cmd, new { id })).SingleOrDefault();
    }

    public Task<IEnumerable<Company>> GetRangeAsync(int from, int to)
    {
        ThrowHelper.ValidateRange(from, to);

        var cmd = $@"select * from {tableName_}
                        order by {nameof(Company.Id)}
                        offset @offset rows
                        fetch next @fetch rows only";
        return connection_.QueryAsync<Company>(cmd, new { Offset = from - 1, Fetch = to - from + 1 });
    }

    public Task UpdateAsync(Company entity)
    {
        var cmd = $"update {tableName_} set {nameof(Company.Name)} = @name where {nameof(Company.Id)} = @id";
        return connection_.ExecuteAsync(cmd, entity);
    }
}
