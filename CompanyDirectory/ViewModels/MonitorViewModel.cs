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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CompanyDirectory.ViewModels
{
    internal class MonitorViewModel : BaseViewModel
    {
        IRepository<Company> _companiesRep;
        IRepository<Employee> _employeesRep;

        public MonitorViewModel(IRepository<Company> companies, IRepository<Employee> employees)
        {
            _companiesRep = companies;
            _employeesRep = employees;
        }

        #region Список компаний
        private CollectionViewSource _companyViewSource;
        private ObservableCollection<CompanyViewItem> _companies;
        public ICollectionView CompanyView => _companyViewSource?.View;
        public ObservableCollection<CompanyViewItem> Companies
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

        #region Список сотрудников
        private ICollection<Employee> _employees;
        public ICollection<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }
        #endregion

        #region Описание

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (!Set(ref _description, value))
                    return;
            }
        }

        private ICommand _selectDataCommand;

        public ICommand SelectDataCommand => _selectDataCommand
            ??= new LambdaCommand(OnSelectCommandExecuted);
        private void OnSelectCommandExecuted(object p)
        {
            if (p == null)
                return;
            string des = string.Empty;
            if (p is Employee)
            {
                Employee employee = p as Employee;
                des = string.Format("Сотрудник {0} {1} {2}. Дата рождения: {3} Дата трудоустройства: {4} Должность: {5} ",
                    employee.LastName, employee.FirstName, employee.SecondName, employee.DateofBorn,
                    employee.DateWorkBegin, employee.CurrentPost?.Caption);
            }
            else if (p is CompanyViewItem)
            {
                CompanyViewItem companyViewItem = p as CompanyViewItem;
                des = string.Format("Компания {0}. Дата основания: {1}",
                    companyViewItem.Caption, companyViewItem.DateCreate);
            }
            else if (p is DivisionViewItem)
            {
                DivisionViewItem divisionViewItem = p as DivisionViewItem;
                des = string.Format("Подразделение {0}",
                    divisionViewItem.Caption);
            }
            Description = des;
        }
        #endregion

        #region Загрузка

        private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadDataCommandExecuted);

        private async Task OnLoadDataCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            //компании
            if (Companies == null)
                Companies = new ObservableCollection<CompanyViewItem>();
            Companies.Clear();


            foreach (Company company in await _companiesRep.Items.ToArrayAsync())
            {
                CompanyViewItem companyVievItem = new CompanyViewItem()
                {
                    Caption = company.Caption,
                    DateCreate = company.DateCreate
                };
                List<DivisionViewItem> Divisions = new List<DivisionViewItem>();
                foreach (Division division in company.Divisions) 
                {
                    DivisionViewItem DivisionViewItem = new DivisionViewItem()
                    {
                        Caption = division.Caption,
                        Employees = company.Employees.Where(E => E.CurrentDivision.Id == division.Id)
                    };

                    Divisions.Add(DivisionViewItem);
                }

                companyVievItem.Divisions = Divisions;


                Companies.Add(companyVievItem);
            }

            
            if (Employees == null)
                Employees = new List<Employee>();
            Employees.Clear();
            Employees = await _employeesRep.Items.ToArrayAsync();
        }
        #endregion

    }
}
