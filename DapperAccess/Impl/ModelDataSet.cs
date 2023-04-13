using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class ModelDataSet : IDataSet<Model>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;
    private readonly string referenceTableName_;

    public ModelDataSet(SqlConnection connection, string tableName, string referenceTableName)
    {
        connection_ = connection;
        tableName_ = tableName;
        referenceTableName_ = referenceTableName;
    }

    public async Task AddAsync(Model entity)
    {
        await EnsureReferenceAsync(entity.Company);

        var cmd = $"insert into {tableName_} values (@id, @name, @year, @companyId)";
        await connection_.ExecuteAsync(cmd, new { entity.Id, entity.Name, entity.Year, CompanyId = entity.Company.Id });
    }

    public Task DeleteAsync(string id)
    {
        var cmd = $"delete from {tableName_} where {nameof(Model.Id)} = @id";
        return connection_.ExecuteAsync(cmd, new { id });
    }

    public Task<IEnumerable<Model>> GetAllAsync()
    {
        var cmd = @$"select m.*, c.* from {tableName_} m
                    join {referenceTableName_} c on m.CompanyId = c.{nameof(Company.Id)}";
        return connection_.QueryAsync<Model, Company, Model>(cmd, (model, company) =>
        {
            model.Company = company;
            return model;
        },
        splitOn: nameof(Company.Id));
    }

    public async Task<Model?> GetAsync(string id)
    {
        var cmd = @$"select m.*, c.* from {tableName_} m
                    join {referenceTableName_} c on m.CompanyId = c.{nameof(Company.Id)}
                    where m.{nameof(Model.Id)} = @id";
        return (await connection_.QueryAsync<Model, Company, Model>(cmd, (model, company) =>
        {
            model.Company = company;
            return model;
        },
        new { id },
        splitOn: nameof(Company.Id))).SingleOrDefault();
    }

    public Task<IEnumerable<Model>> GetRangeAsync(int from, int to)
    {
        ThrowHelper.ValidateRange(from, to);

        var cmd = @$"select m.*, c.* from {tableName_} m
                    join {referenceTableName_} c on m.CompanyId = c.{nameof(Company.Id)}
                    group by m.{nameof(Model.Id)}
                    offset @offset rows
                    fetch next @fetch rows only";
        return connection_.QueryAsync<Model, Company, Model>(cmd, (model, company) =>
        {
            model.Company = company;
            return model;
        },
        new { Offset = from - 1, Fetch = to - from + 1 });
    }

    public async Task UpdateAsync(Model entity)
    {
        await EnsureReferenceAsync(entity.Company);

        var cmd = @$"update {tableName_} 
                    set {nameof(Model.Name)} = @name,
                        {nameof(Model.Year)} = @year,
                        CompanyId = @companyId
                    where {nameof(Model.Id)} = @id";
        await connection_.ExecuteAsync(cmd, new { entity.Id, entity.Name, entity.Year, CompanyId = entity.Company.Id });
    }

    private async Task EnsureReferenceAsync(Company company)
    {
        var cmd = $"select {nameof(Company.Id)} from {referenceTableName_} where {nameof(Company.Name)} = @name";
        var reference = (await connection_.QueryAsync<string>(cmd, new { company.Name })).SingleOrDefault();

        if (reference is null)
        {
            cmd = $"insert into {referenceTableName_} values (@id, @name)";
            await connection_.ExecuteAsync(cmd, company);
        }
        else
        {
            company.Id = reference;
        }
    }
}
