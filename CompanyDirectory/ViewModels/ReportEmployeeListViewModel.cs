using CompanyDirectory.Interfaces;
using CompanyDirectory.Models;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class ReportEmployeeListViewModel : BaseViewModel
    {
        public string Title => "Ведомость по зарплате";
        private IRepository<Company> _repositoryCompany;
        private IRepository<Division> _repositoryDivision;
        private IRepository<Employee> _repositoryEmployee;

        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                Set(ref _selectedCompany, value);
                if (_selectedCompany == null || MainDivisionsList == null)
                    return;
                var filterDivisions = MainDivisionsList.Where(D => _selectedCompany.Divisions.Contains(D));

                Divisions = new ObservableCollection<Division>();
                foreach (Division division in filterDivisions)
                    Divisions.Add(division);
            }
        }

        private Division _selectedDivision;
        public Division SelectedDivision
        {
            get => _selectedDivision;
            set => Set(ref _selectedDivision, value);
        }

        public ReportEmployeeListViewModel()
            : this(null, null, null)
        {

        }

        public ReportEmployeeListViewModel(IRepository<Company> companies, IRepository<Division> divisions, IRepository<Employee> employees)
        {
            _repositoryCompany = companies;
            _repositoryDivision = divisions;
            _repositoryEmployee = employees;
        }


        #region ReportEmployeeList
        private CollectionViewSource _reportEmployeeListViewSource;
        private ObservableCollection<ReportEmployeeList> _reportEmployeeLists;
        public ICollectionView ReportEmployeeListView => _reportEmployeeListViewSource?.View;
        public ObservableCollection<ReportEmployeeList> ReportEmployeeLists
        {
            get => _reportEmployeeLists;
            set
            {
                if (Set(ref _reportEmployeeLists, value))
                {
                    _reportEmployeeListViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(ReportEmployeeList.CompanyCaption), ListSortDirection.Ascending)
                        }
                    };

                    _reportEmployeeListViewSource.View.Refresh();

                    OnPropertyChanged(nameof(ReportEmployeeListView));
                }
            }
        }
        #endregion

        #region Company List
        public List<Company> MainCompanyList { get; set; }
        private CollectionViewSource _companyViewSource;
        private ObservableCollection<Company> _companies;
        public ICollectionView CompanyView => _companyViewSource?.View;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set
            {
                if (Set(ref _companies, value))
                {
                    _companyViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Company.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _companyViewSource.View.Refresh();

                    OnPropertyChanged(nameof(CompanyView));
                }
            }
        }
        #endregion

        #region Division List
        public List<Division> MainDivisionsList { get; set; }
        private CollectionViewSource _divisionViewSource;
        private ObservableCollection<Division> _divisions;
        public ICollectionView DivisionsView => _divisionViewSource?.View;
        public ObservableCollection<Division> Divisions
        {
            get => _divisions;
            set
            {
                if (Set(ref _divisions, value))
                {
                    _divisionViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Division.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _divisionViewSource.View.Refresh();

                    OnPropertyChanged(nameof(DivisionsView));
                }
            }
        }
        #endregion

        #region Employee List
        private List<Employee> _employees;
        public List<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region Load

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadDataCommandCommandExecuted);

        private async Task OnLoadDataCommandCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (MainCompanyList == null)
                MainCompanyList = new List<Company>();
            if (Companies == null)
                Companies = new ObservableCollection<Company>();
            MainCompanyList.Clear();

            foreach (Company company in await _repositoryCompany.Items.ToArrayAsync())
            {
                MainCompanyList.Add(company);
                Companies.Add(company);
            }

            if (MainDivisionsList == null)
                MainDivisionsList = new List<Division>();
            MainDivisionsList.Clear();

            foreach (Division division in await _repositoryDivision.Items.ToArrayAsync())
                MainDivisionsList.Add(division);

            if (Employees == null)
                Employees = new List<Employee>();
            Employees.Clear();

            foreach (Employee employee in await _repositoryEmployee.Items.ToArrayAsync())
                Employees.Add(employee);
        }

        private void UpdateData()
        {
            ReportEmployeeLists = new ObservableCollection<ReportEmployeeList>();

            List<Employee> subList = (List<Employee>)Employees.Where(E => E.CurrentCompany == SelectedCompany && E.CurrentDivision == SelectedDivision);

            foreach (Employee employee in subList)
            {
                ReportModelPayroll reportModelPayroll = new ReportModelPayroll()
                {
                    CompanyCaption = employee.CurrentCompany.Caption,
                    EmployeeFIO = employee.LastName + " " + employee.FirstName + " " + employee.SecondName,
                    DivisionCaption = employee.CurrentDivision.Caption,
                    Salary = employee.Salary
                };
            }
        }
        #endregion
    }
}
