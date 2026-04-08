using System.Windows.Input;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class RegrasViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand VoltarCommand { get; }

        public RegrasViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            VoltarCommand = new RelayCommand(ExecutarVoltar);
        }

        private void ExecutarVoltar(object obj)
        {
            _mainViewModel.NavegarParaLobby();
        }
    }
}