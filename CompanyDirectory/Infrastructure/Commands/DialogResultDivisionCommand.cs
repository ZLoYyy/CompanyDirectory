using CompanyDirectory.Server.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class DialogResultDivisionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool? DialogResult { get; set; }

        public bool CanExecute(object parameter) => App.CurrentWindow != null;

        public void Execute(object parameter)
        {
            if (parameter is bool)
                return;

            if (parameter is Division)
            {
                Division division = parameter as Division;
                if (string.IsNullOrEmpty(division.Caption))
                {
                    MessageBox.Show($"Название не заполнено", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
