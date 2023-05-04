using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using MathCore.Collections;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class ReportManageViewModel : BaseViewModel
    {
        IRepository<Report> _reportsRep;
        IRepository<Company> _companiesRep;
        IRepository<Employee> _employeesRep;
        IRepository<Division> _divisionsRep;
        IRepository<Post> _postsRep;

        public ReportManageViewModel() : this(null, null, null, null, null)
        { }
        public ReportManageViewModel(
            IRepository<Report> reports,
            IRepository<Company> companies,
            IRepository<Employee> employees,
            IRepository<Division> divisions,
            IRepository<Post> posts)
        {
            _reportsRep = reports;
            _companiesRep = companies;
            _employeesRep = employees;
            _divisionsRep = divisions;
            _postsRep = posts;
        }


        #region Список компаний
        private List<Company> _companies;
        public List<Company> Companies
        {
            get => _companies;
            set => Set(ref _companies, value);
        }
        #endregion
        #region Список подразделений
        private List<Division> _divisions;
        public List<Division> Divisions
        {
            get => _divisions;
            set => Set(ref _divisions, value);
        }
        #endregion
        #region Список должностей
        private List<Post> _posts;
        public List<Post> Posts
        {
            get => _posts;
            set => Set(ref _posts, value);
    }
        #endregion
        #region Список Сотрудников
        private List<Employee> _employees;
        public List<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion
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
            if (Companies == null)
                Companies = new List<Company>();
            Companies.Clear();

            foreach (Company company in await _companiesRep.Items.ToArrayAsync())
                Companies.Add(company);

            if (Divisions == null)
                Divisions = new List<Division>();
            Divisions.Clear();

            foreach (Division division in await _divisionsRep.Items.ToArrayAsync())
                Divisions.Add(division);

            if (Posts == null)
                Posts = new List<Post>();
            Posts.Clear();

            foreach (Post post in await _postsRep.Items.ToArrayAsync())
                Posts.Add(post);

            if(Employees == null)
                Employees = new List<Employee>();
            Employees.Clear();

            foreach (Employee employee in await _employeesRep.Items.ToArrayAsync())
                Employees.Add(employee);

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

        /*private ICommand _selectedCompanyCommand;
        public ICommand SelectedCompanyCommand => _selectedCompanyCommand
            ??= new LambdaCommand(OnChangeSelectedCompanyCommandExecuted, CanChangeSelectedCompanyCommandExecute);
        private bool CanChangeSelectedCompanyCommandExecute(object p) => true;

        private void OnChangeSelectedCompanyCommandExecuted(object p) 
        {
            if (CurrentEmployee.CurrentCompany == null)
                return;

            var filterDivisions = Divisions.Select(D => D.Companies.Where(C => C.Id == CurrentEmployee.CurrentCompany.Id));

            Divisions.Clear();
            foreach (Division division in filterDivisions)
                Divisions.Add(division);

        }*/
        #endregion


        #region Построить отчет

        private ICommand _buttonBuildReport;

        public ICommand ButtonBuildReport => _buttonBuildReport
            ??= new LambdaCommandAsync<Report>(OnLoadEmployeeCommandExecuted, CanChangeEditCommandExecute);

        private bool CanChangeEditCommandExecute(Report r) => SelectedReport != null;
        private async Task OnLoadEmployeeCommandExecuted(Report r)
        {
            await BuildReportAsync();
        }

        private async Task BuildReportAsync() 
        {
            /*var postEditorModel = new FilterReportViewModel();

            var postEditorWindow = new FilterReportWindow
            {
                DataContext = postEditorModel
            };*/

           /* if (postEditorWindow.ShowDialog() == true)
                _posts.Add(_repositoryPost.Add(postEditorModel.CurrentPost));

            SelectedPost = postEditorModel.CurrentPost;*/
        }

        #endregion
    }
}
