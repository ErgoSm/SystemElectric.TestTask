using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using SystemElectric.TestTask.Domain.Services;
using SystemElectric.TestTask.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using SystemElectric.TestTask.MsSQL;

namespace SystemElectric.TestTask
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseUrls("http://127.0.0.1:5000")
                .ConfigureServices(services =>
                {
                    services.AddLogging(logger =>
                    {
                        logger.AddFile("app_{0:yyyy}-{0:MM}-{0:dd}_{0:HH}_{1}.log", options =>
                        {
                            options.FormatLogFileName = fName =>
                            {
                                return string.Format(fName, DateTime.Now, DateTime.Now.Minute / 15 + 1);
                            };
                        });
                    });
                    services.AddSingleton<TimeProvider, DefaultTimeProvider>();
                    services.AddSingleton<StorageInteractor>();
                    services.AddSingleton<DataProvider>();
                    services.AddSingleton<DataReader>();
                    services.AddSingleton<ThreadsManager>();
                    services.AddSingleton<EntriesQueueManager>();
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<AdditionalWindow>();
                    services.AddMsSqlRepository("Data Source=;Initial Catalog=test;User ID=;Password=;TrustServerCertificate=True");
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints => endpoints.MapHub<MainHub>("/mainhub"));
                }))
                .Build();
            host.Start();
            var app = host.Services.GetService<App>();
            app?.Run();
        }
    }
}
