using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class CarDataSet : IDataSet<Car>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;
    private readonly string referenceTableName_;
    private readonly string indirectReferenceTableName_;

    public CarDataSet(SqlConnection connection, string tableName, string referenceTableName, string indirectReferenceTableName)
    {
        connection_ = connection;
        tableName_ = tableName;
        referenceTableName_ = referenceTableName;
        indirectReferenceTableName_ = indirectReferenceTableName;
    }

    public async Task AddAsync(Car entity)
    {
        await EnsureReferenceAsync(entity.Model);

        var cmd = $"insert into {tableName_} values (@id, @displacement, @picture, @pictureType, @modelId)";
        await connection_.ExecuteAsync(cmd, new { entity.Id, entity.Displacement, entity.Picture, entity.PictureType, ModelId = entity.Model.Id });
    }

    public Task DeleteAsync(string id)
    {
        var cmd = $"delete from {tableName_} where {nameof(Car.Id)} = @id";
        return connection_.ExecuteAsync(cmd, new { id });
    }

    public Task<IEnumerable<Car>> GetAllAsync()
    {
        var cmd = @$"select c.*, m.*, p.*
                    from {tableName_} c
                    join {referenceTableName_} m on c.ModelId = m.{nameof(Model.Id)}
                    join {indirectReferenceTableName_} p on m.CompanyId = p.{nameof(Company.Id)}";
        return connection_.QueryAsync<Car, Model, Company, Car>(cmd, (car, model, company) =>
        {
            model.Company = company;
            car.Model = model;
            return car;
        },
        splitOn: "Id");
    }

    public async Task<Car?> GetAsync(string id)
    {
        var cmd = @$"select c.*, m.*, p.*
                    from {tableName_} c
                    join {referenceTableName_} m on c.ModelId = m.{nameof(Model.Id)}
                    join {indirectReferenceTableName_} p on m.CompanyId = p.{nameof(Company.Id)}
                    where c.{nameof(Car.Id)} = @id";
        return (await connection_.QueryAsync<Car, Model, Company, Car>(cmd, (car, model, company) =>
        {
            model.Company = company;
            car.Model = model;
            return car;
        },
        new { id },
        splitOn: "Id")).SingleOrDefault();
    }

    public Task<IEnumerable<Car>> GetRangeAsync(int from, int to)
    {
        ThrowHelper.ValidateRange(from, to);

        var cmd = @$"select c.*, m.*, p.*
                    from {tableName_} c
                    join {referenceTableName_} m on c.ModelId = m.{nameof(Model.Id)}
                    join {indirectReferenceTableName_} p on m.CompanyId = p.{nameof(Company.Id)}
                    order by c.{nameof(Car.Id)}
                    offset @offset rows
                    fetch next @fetch rows only";
        return connection_.QueryAsync<Car, Model, Company, Car>(cmd, (car, model, company) =>
        {
            model.Company = company;
            car.Model = model;
            return car;
        },
        new { Offset = from - 1, Fetch = to - from + 1 });
    }

    public async Task UpdateAsync(Car entity)
    {
        await EnsureReferenceAsync(entity.Model);

        var cmd = @$"update {tableName_} 
                    set {nameof(Car.Displacement)} = @displacement,
                        {nameof(Car.Picture)} = @picture,
                        ModelId = @modelId
                    where {nameof(Car.Id)} = @id";
        await connection_.ExecuteAsync(cmd, new { entity.Id, entity.Displacement, entity.Picture, ModelId = entity.Model.Id });
    }

    private async Task EnsureReferenceAsync(Model model)
    {
        var cmd = $"select {nameof(Company.Id)} from {indirectReferenceTableName_} where {nameof(Company.Name)} = @name";
        var reference = (await connection_.QueryAsync<string>(cmd, new { model.Company.Name })).SingleOrDefault();

        if (reference is null)
        {
            cmd = $"insert into {indirectReferenceTableName_} values (@id, @name)";
            await connection_.ExecuteAsync(cmd, model.Company);
        }
        else
        {
            model.Company.Id = reference;
        }

        cmd = @$"select {nameof(Model.Id)} from {referenceTableName_}
                    where {nameof(Model.Name)} = @name
                    and {nameof(Model.Year)} = @year
                    and CompanyId = @companyId";
        reference = (await connection_.QueryAsync<string>(cmd, new { model.Name, model.Year, CompanyId = model.Company.Id })).SingleOrDefault();

        if (reference is null)
        {
            cmd = $"insert into {referenceTableName_} values (@id, @name, @year, @companyId)";
            await connection_.ExecuteAsync(cmd, new { model.Id, model.Name, model.Year, CompanyId = model.Company.Id });
        }
        else
        {
            model.Id = reference;
        }
    }
}
