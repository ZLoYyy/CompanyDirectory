using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CompanyDirectory.Infrastructure.Commands;
using CompanyDirectory.Interfaces;
using CompanyDirectory.Server.Entities;
using CompanyDirectory.ViewModels.Base;
using CompanyDirectory.Views.Windows.SprWondows;
using MathCore.WPF.Commands;
using Microsoft.EntityFrameworkCore;

namespace CompanyDirectory.ViewModels
{
    internal class SprEmployeeViewModel : BaseViewModel
    {
        IRepository<Employee> _repositoryEmployee;
        IRepository<Company> _repositoryCompany;
        IRepository<Division> _repositoryDivision;
        IRepository<Post> _repositoryPost;

        /// <summary>
        /// Выбранная запись
        /// </summary>
        private Employee _selectedEmployee;
        public Employee SelectedEmployee { get => _selectedEmployee; set => Set(ref _selectedEmployee, value); }

        #region Список работников
        private CollectionViewSource _employeesViewSource;
        private ObservableCollection<Employee> _employees;
        public ICollectionView EmployeesView => _employeesViewSource?.View;
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                if (Set(ref _employees, value))
                {
                    _employeesViewSource = new CollectionViewSource
                    {
                        Source = value,
                        SortDescriptions =
                        {
                            new SortDescription(nameof(Employee.LastName), ListSortDirection.Ascending)
                        }
                    };

                    _employeesViewSource.View.Refresh();

                    OnPropertyChanged(nameof(EmployeesView));
                }
            }
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
            var employeeEditorModel = new SprEditEmployeeViewModel(_repositoryCompany, _repositoryDivision, _repositoryPost);

            var employeeEditorWindow = new SprEditEmployeeWindow
            {
                DataContext = employeeEditorModel
            };

            if (employeeEditorWindow.ShowDialog() != true)
                return;

            if (employeeEditorModel.SelectedCompany != null)
                employeeEditorModel.CurrentEmployee.CurrentCompany = employeeEditorModel.SelectedCompany;

            if (employeeEditorModel.SelectedDivision != null)
                employeeEditorModel.CurrentEmployee.CurrentDivision = employeeEditorModel.SelectedDivision;

            if (employeeEditorModel.SelectedPost != null)
                employeeEditorModel.CurrentEmployee.CurrentPost = employeeEditorModel.SelectedPost;


            _employees.Add(_repositoryEmployee.Add(employeeEditorModel.CurrentEmployee));
            SelectedEmployee = employeeEditorModel.CurrentEmployee;
        }

        /// <summary>
        /// Изменить
        /// </summary>
        private ICommand _buttonEditCommand;
        public ICommand ButtonEditCommand => _buttonEditCommand
            ??= new LambdaCommand(OnChangeEditCommandExecuted, CanChangeEditCommandExecute);
        private bool CanChangeEditCommandExecute(object p) => SelectedEmployee != null;

        private void OnChangeEditCommandExecuted(object p)
        {
            var employeeEditorModel = new SprEditEmployeeViewModel(SelectedEmployee, _repositoryCompany, _repositoryDivision, _repositoryPost);

            var employeeEditorWindow = new SprEditEmployeeWindow
            {
                DataContext = employeeEditorModel
            };

            if (employeeEditorWindow.ShowDialog() != true)
                return;
            if (employeeEditorModel.SelectedCompany != null 
                && employeeEditorModel.CurrentEmployee.CurrentCompany != employeeEditorModel.SelectedCompany)
                employeeEditorModel.CurrentEmployee.CurrentCompany = employeeEditorModel.SelectedCompany;

            if (employeeEditorModel.SelectedDivision != null 
                && employeeEditorModel.CurrentEmployee.CurrentDivision != employeeEditorModel.SelectedDivision)
                employeeEditorModel.CurrentEmployee.CurrentDivision = employeeEditorModel.SelectedDivision;

            if (employeeEditorModel.SelectedPost != null 
                && employeeEditorModel.CurrentEmployee.CurrentPost != employeeEditorModel.SelectedPost)
                employeeEditorModel.CurrentEmployee.CurrentPost = employeeEditorModel.SelectedPost;
            
            _repositoryEmployee.Update(employeeEditorModel.CurrentEmployee);
        }
        /// <summary>
        /// Удалить
        /// </summary>
        private ICommand _buttonDeleteCommand;
        public ICommand ButtonDeleteCommand => _buttonDeleteCommand
            ??= new LambdaCommand<Employee>(OnChangeDeleteCommandExecuted, CanChangeDeleteCommandExecute);
        private bool CanChangeDeleteCommandExecute(Employee p) => p != null || SelectedEmployee != null;

        private void OnChangeDeleteCommandExecuted(Employee p)
        {
            var employeeToRemove = p ?? SelectedEmployee;
            if (MessageBox.Show($"Вы хотите удалить сотрудника {employeeToRemove.LastName} {employeeToRemove.FirstName} {employeeToRemove.SecondName}?", "Удаление сотрудника",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            _repositoryEmployee.Remove(employeeToRemove.Id);

            Employees.Remove(employeeToRemove);
            if (ReferenceEquals(SelectedEmployee, employeeToRemove))
                SelectedEmployee = null;
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
            if (Employees == null)
                Employees = new ObservableCollection<Employee>();
            Employees.Clear();

            foreach (Employee employee in await _repositoryEmployee.Items.ToArrayAsync())
                Employees.Add(employee);
        }
        #endregion


        public SprEmployeeViewModel(IRepository<Employee> employees
            ,IRepository<Company> companies
            ,IRepository<Division> divisions
            , IRepository<Post> posts)
        {
            _repositoryCompany = companies;
            _repositoryDivision = divisions;
            _repositoryEmployee = employees;
            _repositoryPost = posts;
        }
    }
}
