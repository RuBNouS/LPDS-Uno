using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private readonly XmlDataService _dataService;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            _dataService = new XmlDataService();
            NavegarParaLobby();
        }

        public void NavegarParaLobby()
        {
            CurrentViewModel = new LobbyViewModel(this, _dataService);
        }

        public void NavegarParaTabuleiro(Jogo jogo, string nomeSaveCarregado = null)
        {
            CurrentViewModel = new TabuleiroViewModel(jogo, _dataService, this, nomeSaveCarregado);
        }

        public void NavegarParaResultados(Jogo jogo)
        {
            CurrentViewModel = new ResultadosViewModel(jogo, _dataService, this);
        }

        public void NavegarParaRegras()
        {
            CurrentViewModel = new RegrasViewModel(this);
        }

        public void NavegarParaSaves()
        {
            CurrentViewModel = new SavesViewModel(this, _dataService);
        }
    }
}