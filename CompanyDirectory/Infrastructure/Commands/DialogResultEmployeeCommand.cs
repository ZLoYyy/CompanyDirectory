using CompanyDirectory.Server.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class DialogResultEmployeeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool? DialogResult { get; set; }

        public bool CanExecute(object parameter) => App.CurrentWindow != null;

        public void Execute(object parameter)
        {
            if (parameter is bool)
                return;

            if (parameter is Employee)
            {
                Employee employee = parameter as Employee;
                if (string.IsNullOrEmpty(employee.FirstName))
                {
                    MessageBox.Show($"Имя не указано", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(employee.LastName))
                {
                    MessageBox.Show($"Фамилия не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(employee.SecondName))
                {
                    MessageBox.Show($"Отчество не указано", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.DateofBorn <= DateTime.MinValue)
                {
                    MessageBox.Show($"Дата рождения не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.DateWorkBegin <= DateTime.MinValue)
                {
                    MessageBox.Show($"Дата ттрудоустройства не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.CurrentCompany == null)
                {
                    MessageBox.Show($"Компания не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.CurrentDivision == null)
                {
                    MessageBox.Show($"Подразделение не указано", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.CurrentPost == null)
                {
                    MessageBox.Show($"Должность не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (employee.Salary < 0)
                {
                    MessageBox.Show($"Зарплата некоректна", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            if (!CanExecute(parameter)) return;


            var window = App.CurrentWindow;
                       
            window.DialogResult = true;
            window.Close();
        }
    }
}
