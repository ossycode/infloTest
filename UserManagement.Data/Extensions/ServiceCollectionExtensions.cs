using Microsoft.EntityFrameworkCore;
using System;
using UserManagement.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDataAccess(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<DataContext>(optionsAction);

        services.AddScoped<IDataContext, DataContext>();

        return services;
    }
}
