using System.Collections.ObjectModel;
using System.Windows.Input;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class SavesViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly XmlDataService _dataService;

        public ObservableCollection<string> ListaSaves { get; } = new ObservableCollection<string>();

        public ICommand CarregarSaveCommand { get; }
        public ICommand ApagarSaveCommand { get; }
        public ICommand VoltarCommand { get; }

        public SavesViewModel(MainViewModel mainViewModel, XmlDataService dataService)
        {
            _mainViewModel = mainViewModel;
            _dataService = dataService;

            CarregarSaveCommand = new RelayCommand(ExecutarCarregarSave);
            ApagarSaveCommand = new RelayCommand(ExecutarApagarSave);
            VoltarCommand = new RelayCommand(o => _mainViewModel.NavegarParaLobby());

            CarregarListaSaves();
        }

        private void CarregarListaSaves()
        {
            ListaSaves.Clear();
            var saves = _dataService.GetSavedGames();
            foreach (var save in saves)
            {
                ListaSaves.Add(save);
            }
        }

        private void ExecutarCarregarSave(object parametro)
        {
            if (parametro is string saveName)
            {
                var jogoGuardado = _dataService.LoadGame(saveName);
                if (jogoGuardado != null)
                {
                    _mainViewModel.NavegarParaTabuleiro(jogoGuardado, saveName);
                }
            }
        }

        private void ExecutarApagarSave(object parametro)
        {
            if (parametro is string saveName)
            {
                _dataService.DeleteSave(saveName);
                CarregarListaSaves();
            }
        }
    }
}