﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemElectric.TestTask.Domain.Entities;
using SystemElectric.TestTask.Domain.Repositories;
using SystemElectric.TestTask.MsSQL.Context;
using SystemElectric.TestTask.MsSQL.Repository;

namespace SystemElectric.TestTask.MsSQL
{
    public static class RepositoryExtensions
    {
        public static void AddMsSqlRepository(this IServiceCollection services)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MainContext>();
            optionsBuilder.UseSqlServer("Data Source=ERGOSM;Initial Catalog=test;User ID=OMR;Password=Ew21Fyon;TrustServerCertificate=True");
            services.AddTransient(context => new MainContext(optionsBuilder.Options));
            services.AddSingleton<IGenericRepository<CarEntry>, GenericRepository<CarEntry>>();
            services.AddSingleton<IGenericRepository<DriverEntry>, GenericRepository<DriverEntry>>();
        }
    }
}