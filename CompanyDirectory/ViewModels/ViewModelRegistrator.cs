using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyDirectory.ViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
           .AddScoped<MainWindowViewModel>()
           .AddScoped<MonitorViewModel>()
           .AddScoped<ReportManageViewModel>()
           .AddScoped<SprCompanyViewModel>()
           .AddScoped<SprDivisionViewModel>()
           .AddScoped<SprEmployeeViewModel>()
           .AddScoped<SprPostViewModel>()
           .AddScoped<SprEditCompanyViewModel>()
           .AddScoped<SprEditDivisionViewModel>()
           .AddScoped<SprEditEmployeeViewModel>()
           .AddScoped<SprEditPostViewModel>()
           .AddScoped<SelectedItemViewModel>()
            ;
    }
}
