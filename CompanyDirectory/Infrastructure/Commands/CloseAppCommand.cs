using CompanyDirectory.Infrastructure.Commands.Base;
using System.Windows;

namespace CompanyDirectory.Infrastructure.Commands
{
    internal class CloseAppCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
