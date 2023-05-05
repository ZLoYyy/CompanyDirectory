using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows.ReportWindows;
using MathCore.Collections;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class ReportManageViewModel : BaseViewModel
    {
        IRepository<Report> _reportsRep;
        IRepository<Company> _companiesRep;
        IRepository<Division> _divisionsRep;
        IRepository<Employee> _employeesRep;

        public ReportManageViewModel() : this(null, null, null, null)
        { }
        public ReportManageViewModel(
            IRepository<Report> reports,
            IRepository<Company> companies, 
            IRepository<Division> divisions, 
            IRepository<Employee> employees)
        {
            _reportsRep = reports;
            _companiesRep = companies;
            _divisionsRep = divisions;
            _employeesRep = employees;
        }
        #region Список oтчетов
        private CollectionViewSource _reportViewSource;
        private ObservableCollection<Report> _reports;
        public ICollectionView ReportView => _reportViewSource?.View;
        public ObservableCollection<Report> Reports
        {
            get => _reports;
            set
            {
                if (Set(ref _reports, value))
                {
                    _reportViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Report.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _reportViewSource.View.Refresh();

                    OnPropertyChanged(nameof(ReportView));
                }
            }
        }
        #endregion


        #region Загрузка

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadEmployeeCommandExecuted);

        private async Task OnLoadEmployeeCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Reports == null)
                Reports = new ObservableCollection<Report>();
            Reports.Clear();

            foreach (Report report in await _reportsRep.Items.ToArrayAsync())
                Reports.Add(report);
        }
        #endregion

        #region Выбор отчета

        private Report _selectedReport;
        public Report SelectedReport { get => _selectedReport; set => Set(ref _selectedReport, value); }
                
        #endregion


        #region Посмотреть отчет

        private ICommand _buttonStartReport;

        public ICommand ButtonStartReport => _buttonStartReport
            ??= new LambdaCommand<Report>(OnLoadEmployeeCommandExecuted, CanChangeEditCommandExecute);

        private bool CanChangeEditCommandExecute(Report r) => SelectedReport != null;
        private void OnLoadEmployeeCommandExecuted(Report r)
        {
            BaseViewModel reportModel = null;
            Window reportWindow = null;
            switch (SelectedReport.Tag)
            {
                case "EmployeesList":
                    reportModel = new ReportEmployeeListViewModel(_companiesRep, _divisionsRep, _employeesRep);

                    reportWindow = new ReportEmployeeListView
                    {
                        DataContext = reportModel
                    };
                    break;
                case "Payroll":
                    reportModel = new ReportPayrollViewModel(_companiesRep, _divisionsRep, _employeesRep);

                    reportWindow = new ReportPayrollView
                    {
                        DataContext = reportModel
                    };
                    break;
            }

            if (reportWindow.ShowDialog() == true)
            { }
        }

        #endregion
    }
}
