using Microsoft.Extensions.DependencyInjection;

namespace CompanyDirectory.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public MonitorViewModel MonitorModel => App.Services.GetRequiredService<MonitorViewModel>();
        public ReportManageViewModel ReportManageModel => App.Services.GetRequiredService<ReportManageViewModel>();
        public SprCompanyViewModel SprCompanyModel => App.Services.GetRequiredService<SprCompanyViewModel>();
        public SprDivisionViewModel SprDivisionModel => App.Services.GetRequiredService<SprDivisionViewModel>();
        public SprEmployeeViewModel SprEmployeeModel => App.Services.GetRequiredService<SprEmployeeViewModel>();
        public SprPostViewModel SprPostModel => App.Services.GetRequiredService<SprPostViewModel>();
    }
}
