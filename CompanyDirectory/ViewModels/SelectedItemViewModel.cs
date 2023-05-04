using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.Server.Entities.Base;
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
    class SelectedItemViewModel : BaseViewModel
    {
        private List<NameEntity> _dataList;
        public List<NameEntity> DataList { get => _dataList; set => Set(ref _dataList, value); }
        public SelectedItemViewModel(List<NameEntity> dataList)
        {
            _dataList = dataList;
        }

///        IRepository<Division> _divisionsRep;

        /// <summary>
        /// Подразделение
        /// </summary>
        private NameEntity _selectedItem;
        public NameEntity SelectedItem { get => _selectedItem; set => Set(ref _selectedItem, value); }
        /// <summary>
        /// Конструктор
        /// </summary>
        /*public SelectedItemViewModel():this(new Company(), null) { }
        public SelectedItemViewModel(Company currentCimpany, IRepository<Division> divisions) 
        {
            _divisionsRep = divisions;
            _currentCompany = currentCimpany;
        }*/
       

        #region Список подразделений
        /*private CollectionViewSource _divisionsViewSource;
        private ObservableCollection<Division> _divisions;
        public ICollectionView DivisionsView => _divisionsViewSource?.View;
        public ObservableCollection<Division> Divisions
        {
            get => _divisions;
            set
            {
                if (Set(ref _divisions, value))
                {
                    _divisionsViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Division.Caption), ListSortDirection.Ascending)
                        }
                    };

                    _divisionsViewSource.View.Refresh();

                    OnPropertyChanged(nameof(DivisionsView));
                }
            }
        }*/
        #endregion

        #region Загрузка

        /*private ICommand _loadDataCommand;

        public ICommand LoadDataCommand => _loadDataCommand
            ??= new LambdaCommandAsync(OnLoadEmployeeCommandExecuted);

        private async Task OnLoadEmployeeCommandExecuted()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Divisions == null)
                Divisions = new ObservableCollection<Division>();
            Divisions.Clear();

            if (CurrentCompany == null)
                return;

            foreach (Division division in await _divisionsRep.Items.Where(D=>!CurrentCompany.Divisions.Contains(D)).ToArrayAsync())
                Divisions.Add(division);
        }*/
        #endregion

    }
}
