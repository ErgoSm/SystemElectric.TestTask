using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;
using SystemElectric.TestTask.MsSQL.Context;
using SystemElectric.TestTask.MsSQL.Repository;

namespace SystemElectric.TestTask.MsSQL
{
    public static class RepositoryExtensions
    {
        public static void AddMsSqlRepository(this IServiceCollection services, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlServer(connectionString);
            services.AddTransient(context => new MainContext(optionsBuilder.Options, new Dictionary<string, string>
            {
                { "DateTime", "datetime" },
                { "String", "nvarchar(50)" }
            }));
            services.AddSingleton<IGenericRepository<CarEntry>, GenericRepository<CarEntry>>();
            services.AddSingleton<IGenericRepository<DriverEntry>, GenericRepository<DriverEntry>>();
        }

        public static void AddNpgSqlRepository(this IServiceCollection services, string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseNpgsql(connectionString);
            services.AddTransient(context => new MainContext(optionsBuilder.Options, new Dictionary<string, string>
            {
                { "DateTime", "timestamp" },
                { "String", "text" }
            },
            () => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true)));
            services.AddSingleton<IGenericRepository<CarEntry>, GenericRepository<CarEntry>>();
            services.AddSingleton<IGenericRepository<DriverEntry>, GenericRepository<DriverEntry>>();
        }
    }
}
