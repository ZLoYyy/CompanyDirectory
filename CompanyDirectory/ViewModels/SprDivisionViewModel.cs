using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows.SprWondows;
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
    internal class SprDivisionViewModel : BaseViewModel
    {
        IRepository<Division> _divisionsRep;
        IRepository<Employee> _employeesRep;
        IRepository<Post> _postRep;

        private Division _selectedDivision;
        public Division SelectedDivision
        {
            get => _selectedDivision;
            set => Set(ref _selectedDivision, value);
        }

        #region Список должностей
        private CollectionViewSource _divisionsViewSource;
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

                    _divisionsViewSource?.View.Refresh();

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
            var divisionEditorModel = new SprEditDivisionViewModel(_postRep);

            var divisionEditorWindow = new SprEditDivisionWindow
            {
                DataContext = divisionEditorModel
            };

            if (divisionEditorWindow.ShowDialog() != true)
                return;

            Divisions.Add(_divisionsRep.Add(divisionEditorModel.CurrentDivision));

            SelectedDivision = divisionEditorModel.CurrentDivision;
        }

        private ICommand _buttonEditCommand;
        public ICommand ButtonEditCommand => _buttonEditCommand
            ??= new LambdaCommand(OnChangeEditCommandExecuted, CanChangeEditCommandExecute);
   
        private bool CanChangeEditCommandExecute(object p) => SelectedDivision != null;

        private void OnChangeEditCommandExecuted(object p)
        {
            var divisionEditorModel = new SprEditDivisionViewModel(SelectedDivision, _postRep);

            var divisionEditorWindow = new SprEditDivisionWindow
            {
                DataContext = divisionEditorModel
            };

            if (divisionEditorWindow.ShowDialog() != true)
                return;

            _divisionsRep.Update(divisionEditorModel.CurrentDivision);
            //SelectedDivision = divisionEditorModel.CurrentDivision;
        }



        private ICommand _buttonDeleteCommand;
        public ICommand ButtonDeleteCommand => _buttonDeleteCommand
            ??= new LambdaCommand<Division>(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);

        private bool CanChangeDeleteCommandExecute(Division d) => SelectedDivision != null;

        private void OnChangeDeleteCommandExecuted(Division d)
        {
            var divisionToRemove = d ?? SelectedDivision;
            if (MessageBox.Show($"Вы хотите удалить подразделение {divisionToRemove.Caption}?", "Удаление подразделения",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _divisionsRep.Remove(divisionToRemove.Id);

            Divisions.Remove(divisionToRemove);
            if (ReferenceEquals(SelectedDivision, divisionToRemove))
                SelectedDivision = null;
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
            if (Divisions == null)
                Divisions = new ObservableCollection<Division>();
            Divisions.Clear();

            foreach (Division division in await _divisionsRep.Items.ToArrayAsync())
                Divisions.Add(division);
        }
        #endregion

        public SprDivisionViewModel(
            IRepository<Division> divisions,
            IRepository<Employee> employees,
            IRepository<Post> postRep)
        {
            _divisionsRep = divisions;
            _employeesRep = employees;
            _postRep = postRep;
        }
    }
}
