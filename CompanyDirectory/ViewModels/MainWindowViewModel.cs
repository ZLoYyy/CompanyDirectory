using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using MathCore.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        IRepository<Company> _companyRep;
        IRepository<Division> _divisionRep;
        IRepository<Employee> _employeesRep;
        IRepository<Post> _postsRep;
        IRepository<Report> _reportRep;

        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrenViewtModel { get => _currentViewModel; private set => Set(ref _currentViewModel, value); }


        #region ChangePages
        private ICommand _buttonMonitorPageCommand;
        public ICommand ButtonMonitorPageCommand => _buttonMonitorPageCommand
            ??= new LambdaCommand(OnChangeMonitorPageCommandExecuted, CanChangeMonitorPageCommandExecute);
        private bool CanChangeMonitorPageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(MonitorViewModel);

        private void OnChangeMonitorPageCommandExecuted(object p)
        {
            CurrenViewtModel = new MonitorViewModel(_companyRep, _divisionRep, _employeesRep, _postsRep);
        }     
        
        private ICommand _buttonSprCompanyPageCommand;
        public ICommand ButtonSprCompanyPageCommand => _buttonSprCompanyPageCommand
            ??= new LambdaCommand(OnChangeSprCompanyPageCommandExecuted, CanChangeSprCompanyPageCommandExecute);
        private bool CanChangeSprCompanyPageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(SprCompanyViewModel);

        private void OnChangeSprCompanyPageCommandExecuted(object p)
        {
            CurrenViewtModel = new SprCompanyViewModel(_companyRep, _divisionRep);
        }

        private ICommand _buttonSprDivisionPageCommand;
        public ICommand ButtonSprDivisionPageCommand => _buttonSprDivisionPageCommand
            ??= new LambdaCommand(OnChangeSprDivisionPageCommandExecuted, CanChangeSprDivisionPageCommandExecute);
        private bool CanChangeSprDivisionPageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(SprDivisionViewModel);

        private void OnChangeSprDivisionPageCommandExecuted(object p)
        {
            CurrenViewtModel = new SprDivisionViewModel(_divisionRep, _employeesRep, _postsRep);
        }

        private ICommand _buttonSprEmployeePageCommand;
        public ICommand ButtonSprEmployeePageCommand => _buttonSprEmployeePageCommand
            ??= new LambdaCommand(OnChangeSprEmployeePageCommandExecuted, CanChangeSprEmployeePageCommandExecute);
        private bool CanChangeSprEmployeePageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(SprEmployeeViewModel);

        private void OnChangeSprEmployeePageCommandExecuted(object p)
        {
            CurrenViewtModel = new SprEmployeeViewModel(_employeesRep, _companyRep, _divisionRep, _postsRep);
        }

        private ICommand _buttonSprPostPageCommand;
        public ICommand ButtonSprPostPageCommand => _buttonSprPostPageCommand
            ??= new LambdaCommand(OnChangeSprPostPageCommandExecuted, CanChangeSprPostPageCommandExecute);

        private bool CanChangeSprPostPageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(SprPostViewModel);

        private void OnChangeSprPostPageCommandExecuted(object p)
        {
            CurrenViewtModel = new SprPostViewModel(_postsRep);
        }

        private ICommand _buttonReportPageCommand;
        public ICommand ButtonReportPageCommand => _buttonReportPageCommand
            ??= new LambdaCommand(OnChangeReportPageCommandExecuted, CanChangeReportPageCommandExecute);
        private bool CanChangeReportPageCommandExecute(object p) => CurrenViewtModel.GetType() != typeof(ReportManageViewModel);

        private void OnChangeReportPageCommandExecuted(object p)
        {
            CurrenViewtModel = new ReportManageViewModel(_reportRep, _companyRep, _divisionRep, _employeesRep);
        }
        #endregion

        public MainWindowViewModel(
            IRepository<Company> companies,
            IRepository<Employee> employees,
            IRepository<Division> divisions,
            IRepository<Post> posts,
            IRepository<Report> reports)
        {
            _companyRep = companies;
            _employeesRep= employees;
            _divisionRep = divisions;
            _postsRep = posts;
            _reportRep = reports;

            CurrenViewtModel = new MonitorViewModel(_companyRep, _divisionRep, _employeesRep, _postsRep);
        }
        
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get => "Реестр компаний"; }
        /// <summary>
        /// Текущая страница
        /// </summary>
          
    }
}
