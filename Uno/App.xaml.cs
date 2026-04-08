using System.Windows;
using Uno.Views;
using Uno.ViewModels;

namespace Uno
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(); // Injeta o ViewModel Principal
            mainWindow.Show();
        }
    }
}