using CompanyDirectory.Data;
using CompanyDirectory.Services;
using CompanyDirectory.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyDirectory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window ActiveWindow => Application.Current.Windows
               .OfType<Window>()
               .FirstOrDefault(w => w.IsActive);

        public static Window FocusedWindow => Application.Current.Windows
           .OfType<Window>()
           .FirstOrDefault(w => w.IsFocused);

        public static Window CurrentWindow => FocusedWindow ?? ActiveWindow;

        private static IHost _host;
        public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration.GetSection("DataBase"))
            //.AddServices()
            .AddViewModels();

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            using (var scope = Services.CreateScope())
                scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync().Wait();

            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var host = Host;
            base.OnExit(e);
            await host.StopAsync();
        }
    }
}
