using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.Server.Entities.Base;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows;
using CompanyDirectory.Views.Windows.SprWondows;
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
    internal class SprEditCompanyViewModel : BaseViewModel
    {
        public string Title
        {
            get
            {
                if (CurrentCompany != null && CurrentCompany.Id > 0)
                    return "Редактирование компнии";
                else
                    return "Добавление компнии";
            }
        }
        private IRepository<Division> _repositoryDivision;


        private Company _company;
        public Company CurrentCompany { get => _company; set => Set(ref _company, value); }

        public SprEditCompanyViewModel()
        : this(new Company(), null) { }

        public SprEditCompanyViewModel(IRepository<Division> repositoryDivision)
            : this(new Company(), repositoryDivision)
        {
        }

        public SprEditCompanyViewModel(Company company, IRepository<Division> repositoryDivision)
        {
            _company = company;
            _repositoryDivision = repositoryDivision;

            if (_company != null)
            {
                if (VievDivisions == null)
                    VievDivisions = new ObservableCollection<Division>();

                foreach (Division division in _company.Divisions)
                    VievDivisions.Add(division);
            }
        }

        #region Список подразделений
        /// <summary>
        /// Общий список
        /// </summary>
        public List<Division> Divisions { get; set; }

        /// <summary>
        /// Для отображения
        /// </summary>
        private CollectionViewSource _divisionsViewSource;
        private ObservableCollection<Division> _divisions;
        public ICollectionView DivisionsView => _divisionsViewSource?.View;
        public ObservableCollection<Division> VievDivisions
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
            if (Divisions == null)
                Divisions = new List<Division>();
            Divisions.Clear();

            if (CurrentCompany == null)
                return;

            foreach (Division division in await _repositoryDivision.Items.Where(D => !D.Companies.Contains(CurrentCompany)).ToArrayAsync())
                Divisions.Add(division);
        }
        #endregion

        #region Кнопки
        /// <summary>
        /// Создать
        /// </summary>
        private ICommand _buttonAddCommand;
        public ICommand ButtonAddCommand => _buttonAddCommand
            ??= new LambdaCommand(OnChangeAddCommandExecuted, CanChangeAddCommandExecute);
        private bool CanChangeAddCommandExecute(object p) => true;

        private void OnChangeAddCommandExecuted(object p)
        {
            var divisionEditorModel = new SelectedItemViewModel(Divisions.ToList<NameEntity>(), "Выбор подразделения");

            var divisionEditorWindow = new SelectedItemWindow
            {
                DataContext = divisionEditorModel
            };

            if (divisionEditorWindow.ShowDialog() != true || divisionEditorModel.SelectedItem == null)
                return;
            VievDivisions.Add((Division)divisionEditorModel.SelectedItem);
            CurrentCompany.Divisions.Add((Division)divisionEditorModel.SelectedItem);
        }
        
        /// <summary>
        /// Удалить
        /// </summary>
        private ICommand _buttonDeleteCommand;
        public ICommand ButtonDeleteCommand => _buttonDeleteCommand
            ??= new LambdaCommand<Division>(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);
        private bool CanChangeDeleteCommandExecute(Division p) => SelectedDivision != null;

        private void OnChangeDeleteCommandExecuted(Division p)
        {
            var divisionToRemove = p ?? SelectedDivision;
            if (MessageBox.Show($"Вы хотите удалить подразделение {divisionToRemove.Caption}?", "Удаление работника",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            if (CurrentCompany.Divisions.Select(D => D.Id == divisionToRemove.Id) != null)
                CurrentCompany.Divisions.Remove(divisionToRemove);
            if (VievDivisions.Select(D => D.Id == divisionToRemove.Id) != null)
                VievDivisions.Remove(divisionToRemove);
            if (ReferenceEquals(SelectedDivision, divisionToRemove))
                SelectedDivision = null;
        }
        #endregion

        private Division _selectedDivision;
        public Division SelectedDivision
        {
            get => _selectedDivision;
            set => Set(ref _selectedDivision, value);

        }
    }
}
