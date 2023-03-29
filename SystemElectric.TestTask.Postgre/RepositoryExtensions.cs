using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;
using SystemElectric.TestTask.Postgre.Context;
using SystemElectric.TestTask.Postgre.Repository;

namespace SystemElectric.TestTask.Postgre
{
    public static class RepositoryExtensions
    {
        public static void AddNpgSqlRepository(this IServiceCollection services, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseNpgsql(connectionString);
            services.AddTransient(context => new MainContext(optionsBuilder.Options));
            services.AddSingleton<IGenericRepository<CarEntry>, GenericRepository<CarEntry>>();
            services.AddSingleton<IGenericRepository<DriverEntry>, GenericRepository<DriverEntry>>();
        }
    }
}