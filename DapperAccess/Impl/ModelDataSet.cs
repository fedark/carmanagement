using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Data.Access.Abstractions;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace DapperAccess.Impl;
public class ModelDataSet : IDataSet<Model>
{
    private readonly SqlConnection connection_;
    private readonly string tableName_;

    public ModelDataSet(SqlConnection connection, string tableName)
    {
        connection_ = connection;
        tableName_ = tableName;
    }

    public async Task<Model?> GetAsync(string id)
    {
        var cmd = $"select * from {tableName_} where Id = @id";
        return (await connection_.QueryAsync<Model>(cmd, new { Id = id })).SingleOrDefault();
    }
}
