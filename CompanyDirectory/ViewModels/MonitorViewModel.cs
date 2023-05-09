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

        public MonitorViewModel(IRepository<Company> companies)
        {
            _companiesRep = companies;
        }

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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (!Set(ref _isSelected, value))
                    return;
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (!Set(ref _isExpanded, value))
                    return;
            }
        }

        #region Список компаний
        private CollectionViewSource _companyViewSource;
        private ObservableCollection<CompanyVievItem> _companies;
        public ICollectionView CompanyView => _companyViewSource?.View;
        public ObservableCollection<CompanyVievItem> Companies
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
                Companies = new ObservableCollection<CompanyVievItem>();
            Companies.Clear();


            foreach (Company company in await _companiesRep.Items.ToArrayAsync())
            {
                CompanyVievItem companyVievItem = new CompanyVievItem()
                {
                    Caption = company.Caption
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
        }
        #endregion

    }
}
