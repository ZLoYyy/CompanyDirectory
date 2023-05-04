using CompanyDirectory.Server;
using CompanyDirectory.Server.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CompanyDirectory.Data
{
    internal static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
           .AddDbContext<CompanyDirectoryDBContext>(optionsBuilder =>
           {
               var type = Configuration["Type"];
               switch (type)
               {
                   case null: throw new InvalidOperationException("Не определён тип БД");
                   default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");                       

                   case "MSSQL":
                       optionsBuilder.UseSqlServer(Configuration.GetConnectionString(type));
                       break;
               }
           })
            .AddTransient<DbInitializer>()
            .AddRepositoriesInDB();
    }
}
