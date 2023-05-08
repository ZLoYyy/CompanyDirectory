using CompanyDirectory.Interfaces;
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
    internal class SprEditEmployeeViewModel : BaseViewModel
    {
        public string Title
        {
            get
            {
                if (CurrentEmployee != null && CurrentEmployee.Id > 0)
                    return "Редактирование сотрудника";
                else
                    return "Добавление сотрудника";
            }
        }

        private IRepository<Company> _repositoryCompany;
        private IRepository<Division> _repositoryDivision;
        private IRepository<Post> _repositoryPosts;

        private Employee _currentEmployee;

        public Employee CurrentEmployee { get => _currentEmployee; set => Set(ref _currentEmployee, value); }


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


                CurrentEmployee.CurrentCompany = _selectedCompany;
                Divisions = new ObservableCollection<Division>();
                foreach (Division division in filterDivisions)
                    Divisions.Add(division);
            }
        }

        private Division _selectedDivision;
        public Division SelectedDivision 
        { 
            get => _selectedDivision; 
            set
            {
                Set(ref _selectedDivision, value);

                if (_selectedDivision == null || MainPostList == null)
                    return;

                CurrentEmployee.CurrentDivision = _selectedDivision;
                var filterPosts = MainPostList.Where(P => _selectedDivision.Posts.Contains(P));

                Posts = new ObservableCollection<Post>();
                foreach (Post post in filterPosts)
                    Posts.Add(post);
            }
        }


        private Post _selectedPost;
        public Post SelectedPost { get => _selectedPost; set 
            {
                if (!Set(ref _selectedPost, value))
                    return;

                CurrentEmployee.CurrentPost = _selectedPost;
            } 
        }
        public SprEditEmployeeViewModel() : this(null, null, null, null) { }
        public SprEditEmployeeViewModel(IRepository<Company> repositoryCompany, IRepository<Division> repositoryDivision, IRepository<Post> repositoryPosts)
            : this(new Employee(), repositoryCompany, repositoryDivision, repositoryPosts)
        {
        }

        public SprEditEmployeeViewModel(Employee employee
            , IRepository<Company> repositoryCompany
            ,IRepository<Division> repositoryDivision
            ,IRepository<Post> repositoryPosts)
        {
            _currentEmployee = employee;
            _repositoryCompany = repositoryCompany;
            _repositoryDivision = repositoryDivision;
            _repositoryPosts = repositoryPosts;
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

        #region Список должностей
        public List<Post> MainPostList { get; set; }
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
                Companies = new ObservableCollection<Company>();
            Companies.Clear();

            foreach (Company company in await _repositoryCompany.Items.ToArrayAsync())
                Companies.Add(company);

            if (MainDivisionsList == null)
                MainDivisionsList = new List<Division>();
            MainDivisionsList.Clear();

            foreach (Division division in await _repositoryDivision.Items.ToArrayAsync())
                MainDivisionsList.Add(division);

            if (MainPostList == null)
                MainPostList = new List<Post>();
            MainPostList.Clear();

            foreach (Post post in await _repositoryPosts.Items.ToArrayAsync())
                MainPostList.Add(post);

            if (_currentEmployee != null)
            {
                SelectedCompany = _currentEmployee.CurrentCompany;
                SelectedDivision = _currentEmployee.CurrentDivision;
                SelectedPost = _currentEmployee.CurrentPost;
            }
        }
        #endregion
               
    }
}
