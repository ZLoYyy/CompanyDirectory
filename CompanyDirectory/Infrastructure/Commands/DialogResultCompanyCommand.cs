using CompanyDirectory.Server.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class DialogResultCompanyCommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool? DialogResult { get; set; }

        public bool CanExecute(object parameter) => App.CurrentWindow != null;

        public void Execute(object parameter)
        {
            if (parameter is bool)
                return;

            if (parameter is Company)
            {
                Company company = parameter as Company;
                if (string.IsNullOrEmpty(company.Caption))
                {
                    MessageBox.Show($"Название не заполнено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (company.DateCreate <= DateTime.MinValue)
                {
                    MessageBox.Show($"Дата сознания не указана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(company.LegalAddress))
                {
                    MessageBox.Show($"Адресс не указан", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
