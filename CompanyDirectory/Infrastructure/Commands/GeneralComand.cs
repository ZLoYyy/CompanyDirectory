using CompanyDirectory.Infrastructure.Commands.Base;
using CompanyDirectory.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class GeneralComand : BaseCommand
    {

        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public GeneralComand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public override void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _execute(parameter);
        }
    }
}
