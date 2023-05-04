using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
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
    internal class MonitorViewModel : BaseViewModel
    {
        IRepository<Company> _companiesRep;
        IRepository<Division> _divisionsRep;
        IRepository<Employee> _employeesRep;
        IRepository<Post> _postsRep;

        public MonitorViewModel(IRepository<Company> companies, 
            IRepository<Division> divisions, 
            IRepository<Employee> employees, 
            IRepository<Post> posts)
        {
            _companiesRep = companies;
            _divisionsRep = divisions;
            _employeesRep = employees;
            _postsRep = posts;
        }

        #region Список компаний
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
        #region Список подразделений
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
        #region Список должностей
        private CollectionViewSource _postViewSource;
        private ObservableCollection<Post> _posts;
        public ICollectionView PostView => _postViewSource?.View;
        public ObservableCollection<Post> Posts
        {
            get => _posts;
            set
            {
                if (Set(ref _posts, value))
                {
                    _postViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Post.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _postViewSource.View.Refresh();

                    OnPropertyChanged(nameof(PostView));
                }
            }
        }
        #endregion
        #region Список сотрудников
        private CollectionViewSource _employeeViewSource;
        private ObservableCollection<Employee> _employees;
        public ICollectionView EmployeeView => _employeeViewSource?.View;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                if (Set(ref _employees, value))
                {
                    _employeeViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Employee.SecondName), ListSortDirection.Ascending)
                        }
                    };

                    _employeeViewSource.View.Refresh();

                    OnPropertyChanged(nameof(EmployeeView));
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
            //компании
            if (Companies == null)
                Companies = new ObservableCollection<Company>();
            Companies.Clear();

            foreach (Company company in await _companiesRep.Items.ToArrayAsync())
                Companies.Add(company);

            //подразделения
            if (Divisions == null)
                Divisions = new ObservableCollection<Division>();
            Divisions.Clear();

            foreach (Division division in await _divisionsRep.Items.ToArrayAsync())
                Divisions.Add(division);

            //должности
            if (Posts == null)
                Posts = new ObservableCollection<Post>();
            Posts.Clear();

            foreach (Post post in await _postsRep.Items.ToArrayAsync())
                Posts.Add(post);

            //Сотрудники
            if (Employees == null)
                Employees = new ObservableCollection<Employee>();
            Employees.Clear();

            foreach (Employee employee in await _employeesRep.Items.ToArrayAsync())
                Employees.Add(employee);
        }
        #endregion


        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        SelectedItem = this;
                    }
                }
            }
        }

        private static object _selectedItem = null;
        public static object SelectedItem
        {
            get { return _selectedItem; }
            private set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnSelectedItemChanged();
                }
            }
        }

        static void OnSelectedItemChanged()
        {
            // Raise event / do other things
        }
    }
}
