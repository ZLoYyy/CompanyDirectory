using CompanyDirectory.Infrastructure.Commands.Base;
using CompanyDirectory.Views.Windows;
using System;
using System.Windows;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class ReportManageCommand : BaseCommand
    {
        private Window _window;

        public override bool CanExecute(object parameter) => _window == null;

        public override void Execute(object parameter)
        {
            
        }

        private void OnWindowClosed(object Sender, EventArgs E)
        {
            ((Window)Sender).Closed -= OnWindowClosed;
            _window = null;
        }
    }
}
