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
                if (Set(ref _selectedCompany, value))
                    UpdateData();
            }
        }

        private string _selectedExperience;
        public string SelectedExperience
        {
            get => _selectedExperience;
            set
            {
                if (Set(ref _selectedExperience, value))
                    UpdateData();
            }
        }

        private string _selectedFilterType;
        public string SelectedFilterType
        {
            get => _selectedFilterType;
            set
            {
                if (!Set(ref _selectedFilterType, value))
                    return;

                if (FilterValueViews == null)
                    FilterValueViews = new ObservableCollection<string>();
                else
                    FilterValueViews.Clear();
                
                //Определяем фильтр
                if (_selectedFilterType.Contains("Возраст")) 
                {
                    FilterValueViews.AddRange(new string[] { "10-20", "20-30", "30-40", "50-60", "60+"});
                }   
                else if (_selectedFilterType.Contains("Год рождения")) 
                {
                    for (int i = DateTime.Today.Year; i >= 1900; i--)
                        FilterValueViews.Add(i.ToString());
                }
            }
        }

        private string _selectedFilterValue;
        public string SelectedFilterValue
        {
            get => _selectedFilterValue;
            set 
            {
                if(Set(ref _selectedFilterValue, value))
                    UpdateData(); 
            }
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

        #region Experience List
        private CollectionViewSource _experienceViewSource;
        private ObservableCollection<string> _experienceViews;
        public ICollectionView ExperienceViewSource => _experienceViewSource?.View;
        public ObservableCollection<string> ExperienceViews
        {
            get => _experienceViews;
            set
            {
                if (Set(ref _experienceViews, value))
                {
                    _experienceViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(value), ListSortDirection.Ascending)
                        }
                    };

                    _experienceViewSource.View.Refresh();

                    OnPropertyChanged(nameof(ExperienceViewSource));
                }
            }
        }
        #endregion

        #region FilterTypesViews List
        private CollectionViewSource _filterTypesViewSource;
        private ObservableCollection<string> _filterTypesViews = new ObservableCollection<string>() { "Год рождения", "Возраст" };
        public ICollectionView FilterTypesView => _filterTypesViewSource?.View;
        public ObservableCollection<string> FilterTypesViews
        {
            get => _filterTypesViews;
            set
            {
                if (Set(ref _filterTypesViews, value))
                {
                    _filterTypesViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Division.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _filterTypesViewSource.View.Refresh();

                    OnPropertyChanged(nameof(FilterTypesView));
                }
            }
        }
        #endregion

        #region ExperienceValueViews List
        private CollectionViewSource _filterValueViewSource;
        private ObservableCollection<string> _filterValueViews;
        public ICollectionView FilterValueView => _filterValueViewSource?.View;
        public ObservableCollection<string> FilterValueViews
        {
            get => _filterValueViews;
            set
            {
                if (Set(ref _filterValueViews, value))
                {
                    _filterValueViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(value), ListSortDirection.Ascending)
                        }
                    };

                    _filterValueViewSource.View.Refresh();

                    OnPropertyChanged(nameof(FilterTypesView));
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

            if (ExperienceViews == null)
                ExperienceViews = new ObservableCollection<string>();
            ExperienceViews.AddRange(new string[] { "0-3", "3-6", "6-10", "10-15", "15-20", "20-30", "30+" });

            foreach (Company company in await _repositoryCompany.Items.ToArrayAsync())
            {
                MainCompanyList.Add(company);
                Companies.Add(company);
            }

            if (Employees == null)
                Employees = new List<Employee>();
            Employees.Clear();

            foreach (Employee employee in await _repositoryEmployee.Items.ToArrayAsync())
                Employees.Add(employee);
        }

        private void UpdateData()
        {
            ReportEmployeeLists = new ObservableCollection<ReportEmployeeList>();
            IEnumerable<Employee> subList = null;
            if (SelectedCompany == null)
                subList = Employees;
            else
                subList = Employees.Where(E => E.CurrentCompany == SelectedCompany);

            if (SelectedExperience != null)
            {
                if (SelectedExperience.Contains("+"))
                {
                    int.TryParse(SelectedExperience.Replace("+", ""), out int experience);
                    subList = Employees.Where(E => GetAge(E.DateWorkBegin) > experience);
                }
                else
                {
                    string[] ages = SelectedExperience.Split("-");
                    if (ages.Length > 1)
                    {
                        int.TryParse(ages[0], out int ageBegin);
                        int.TryParse(ages[1], out int ageEnd);
                        if (ageEnd > 0)
                            subList = subList.Where(E => GetAge(E.DateWorkBegin) >= ageBegin && GetAge(E.DateWorkBegin) <= ageEnd);
                    }
                }
            }

            if (SelectedFilterType != null) 
            {
                if (SelectedFilterType.Contains("Год рождения") && SelectedFilterValue != null)
                {
                    int.TryParse(SelectedFilterValue, out int year);
                    if (year > 0)
                        subList = subList.Where(E => E.DateofBorn.Year >= year);
                }
                else if (SelectedFilterType.Contains("Возраст") && SelectedFilterValue != null)
                {
                    if (SelectedFilterValue.Contains("+"))
                    {
                        int.TryParse(SelectedFilterValue.Replace("+", ""), out int age);
                        subList = subList.Where(E => GetAge(E.DateofBorn) > age);
                    }
                    else
                    {
                        string[] ages = SelectedFilterValue.Split("-");
                        if (ages.Length > 1)
                        {
                            int.TryParse(ages[0], out int ageBegin);
                            int.TryParse(ages[1], out int ageEnd);
                            if (ageEnd > 0)
                                subList = subList.Where(E => GetAge(E.DateofBorn) >= ageBegin && GetAge(E.DateofBorn) <= ageEnd);
                        }
                    }
                }
            }
            



            
            foreach (Employee employee in subList)
            {
                ReportEmployeeList reportModelPayroll = new ReportEmployeeList()
                {
                    CompanyCaption = employee.CurrentCompany.Caption,
                    EmployeeFIO = employee.LastName + " " + employee.FirstName + " " + employee.SecondName,
                    DivisionCaption = employee.CurrentDivision.Caption,
                    Experience = GetAge(employee.DateWorkBegin),
                    Age = GetAge(employee.DateofBorn)
                };

                ReportEmployeeLists.Add(reportModelPayroll);
            }
        }
        /// <summary>
        /// расчет возраста
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <returns></returns>
        private int GetAge(DateTime dateBegin) 
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateBegin.Year;
            if (today.Month < dateBegin.Month || (today.Month == dateBegin.Month && today.Day < dateBegin.Day))
                age--;
            return age;
        }
        #endregion
    }
}
