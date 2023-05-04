using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.Interfaces;
using MathCore.WPF.Commands;
using CompanyDirectory.Views.Windows.SprWondows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CompanyDirectory.ViewModels
{
    internal class SprCompanyViewModel : BaseViewModel
    {
        IRepository<Company> _companiesRep;
        IRepository<Division> _divisionsRep;


        /// <summary>
        /// Выбранная запись
        /// </summary>
        private Company _selectedCompany;
        public Company SelectedCompany { 
            get => _selectedCompany; 
            set                  
            {
                Set(ref _selectedCompany, value);
                if (_selectedCompany == null)
                    return;

                Divisions = new ObservableCollection<Division>();

                foreach (Division division in _selectedCompany.Divisions)
                    Divisions.Add(division);
            }
        }

        #region Список Компаний
        private CollectionViewSource _companyViewSource;
        private ObservableCollection<Company> _companies;
        public ICollectionView CompaniesView => _companyViewSource?.View;
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

                    _companyViewSource?.View.Refresh();

                    OnPropertyChanged(nameof(CompaniesView));
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

                    _divisionViewSource?.View.Refresh();

                    OnPropertyChanged(nameof(DivisionsView));
                }
            }
        }
        #endregion

        #region Buttons


        private ICommand _buttonAddCommand;
        public ICommand ButtonAddCommand => _buttonAddCommand
            ??= new LambdaCommand(OnChangeAddCommandExecuted, CanChangeAddCommandExecute);
        private bool CanChangeAddCommandExecute(object p) => true;

        private void OnChangeAddCommandExecuted(object p)
        {
            var companyEditorModel = new SprEditCompanyViewModel(_divisionsRep);

            var companyEditorWindow = new SprEditCompanyWindow
            {
                DataContext = companyEditorModel
            };

            if (companyEditorWindow.ShowDialog() != true)
                return;

            Companies.Add(_companiesRep.Add(companyEditorModel.CurrentCompany));

            SelectedCompany = companyEditorModel.CurrentCompany;
        }

        private ICommand _buttonEditCommand;
        public ICommand ButtonEditCommand => _buttonEditCommand
            ??= new LambdaCommand(OnChangeEditCommandExecuted, CanChangeEditCommandExecute);

        private bool CanChangeEditCommandExecute(object p) => SelectedCompany != null;

        private void OnChangeEditCommandExecuted(object p)
        {
            //_sprEditDivision.ShowDialog();
            var companyEditorModel = new SprEditCompanyViewModel(SelectedCompany, _divisionsRep);

            var companyEditorWindow = new SprEditCompanyWindow
            {
                DataContext = companyEditorModel
            };

            if (companyEditorWindow.ShowDialog() != true)
                return;

            //Companies.Add(_companiesRep.Add(companyEditorModel.CurrentCompany));
            _companiesRep.Update(companyEditorModel.CurrentCompany);
            SelectedCompany = companyEditorModel.CurrentCompany;
        }

        private ICommand _buttonDeleteCommand;
        public ICommand ButtonDeleteCommand => _buttonDeleteCommand
            ??= new LambdaCommand<Company>(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);

        private bool CanChangeDeleteCommandExecute(Company c) => SelectedCompany != null;

        private void OnChangeDeleteCommandExecuted(Company c)
        {
            var companyToRemove = c ?? SelectedCompany;
            if (MessageBox.Show($"Вы хотите удалить компанию {companyToRemove.Caption}?", "Удаление компании",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _companiesRep.Remove(companyToRemove.Id);

            Companies.Remove(companyToRemove);
            if (ReferenceEquals(SelectedCompany, companyToRemove))
                SelectedCompany = null;
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
            if (Companies == null)
                Companies = new ObservableCollection<Company>();
            Companies.Clear();

            foreach (Company company in await _companiesRep.Items.ToArrayAsync())
                Companies.Add(company);
        }
        #endregion

        public SprCompanyViewModel(IRepository<Company> companies, IRepository<Division> divisions)
        {
            _companiesRep = companies;
            _divisionsRep = divisions;
        }
    }
}
