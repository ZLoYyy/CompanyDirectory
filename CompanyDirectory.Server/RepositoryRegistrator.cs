using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.Server.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.Server
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositoriesInDB(this IServiceCollection services) => services
           .AddTransient<IRepository<Company>, CompanyRepository>()
           .AddTransient<IRepository<Division>, DivisionRepository>()
           .AddTransient<IRepository<Employee>, EmployeeRepository>()
           .AddTransient<IRepository<Post>, DbRepository<Post>>()
           .AddTransient<IRepository<Report>, DbRepository<Report>>();
    }
}
