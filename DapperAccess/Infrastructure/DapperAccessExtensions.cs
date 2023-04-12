using DapperAccess.Impl;
using Data.Access.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DapperAccess.Infrastructure;
public static class DapperAccessExtensions
{
    public static IServiceCollection AddDapperDataContext(this IServiceCollection services, string connectionString)
    {
        services.TryAddScoped<IDataContext>(services => new DapperDataContext(connectionString));
        return services;
    }
}
