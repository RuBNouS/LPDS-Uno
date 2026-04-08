using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly XmlDataService _dataService;

        private ComboBoxItem _numeroBotsSelecionado;
        public ComboBoxItem NumeroBotsSelecionado
        {
            get => _numeroBotsSelecionado;
            set { _numeroBotsSelecionado = value; OnPropertyChanged(); }
        }

        public ICommand CriarJogoCommand { get; }
        public ICommand RetomarPartidaCommand { get; }
        public ICommand VerRegrasCommand { get; }
        public ICommand VerEstatisticasCommand { get; }

        public LobbyViewModel(MainViewModel mainViewModel, XmlDataService dataService)
        {
            _mainViewModel = mainViewModel;
            _dataService = dataService;

            CriarJogoCommand = new RelayCommand(ExecutarCriarJogo);
            RetomarPartidaCommand = new RelayCommand(ExecutarRetomarPartida, PodeRetomarPartida);
            VerRegrasCommand = new RelayCommand(o => MessageBox.Show("Regras do UNO: Fica sem cartas na mão! (Aqui abririas outra View)", "Regras", MessageBoxButton.OK, MessageBoxImage.Information));
            VerEstatisticasCommand = new RelayCommand(o => MessageBox.Show("Estatísticas ainda em construção.", "Estatísticas", MessageBoxButton.OK, MessageBoxImage.Information));
        }

        private void ExecutarCriarJogo(object parametro)
        {
            int numeroBots = 1; // Padrão

            if (NumeroBotsSelecionado?.Content != null && int.TryParse(NumeroBotsSelecionado.Content.ToString(), out int parsedBots))
            {
                numeroBots = parsedBots;
            }

            _mainViewModel.IniciarNovoJogo(numeroBots);
        }

        private bool PodeRetomarPartida(object parametro)
        {
            // O botão de retomar só fica clicável se o ficheiro XML existir
            return _dataService.HasSavedGame();
        }

        private void ExecutarRetomarPartida(object parametro)
        {
            try
            {
                var jogoSuspenso = _dataService.LoadGame();
                if (jogoSuspenso != null)
                {
                    _mainViewModel.NavegarParaTabuleiro(jogoSuspenso);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o jogo salvo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}