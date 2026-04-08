using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class LobbyViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private readonly XmlDataService _dataService;

        public int NumeroBots { get; set; } = 1;
        public ObservableCollection<int> OpcoesBots { get; } = new ObservableCollection<int> { 1, 2, 3 };

        public bool PodeRetomar => _dataService.HasSavedGame();

        public ICommand CriarNovoJogoCommand { get; }
        public ICommand RetomarPartidaCommand { get; }
        public ICommand VerRegrasCommand { get; }
        public ICommand ConsultarEstatisticasCommand { get; }

        public LobbyViewModel(MainViewModel mainViewModel, XmlDataService dataService)
        {
            _mainViewModel = mainViewModel;
            _dataService = dataService;

            CriarNovoJogoCommand = new RelayCommand(ExecutarCriarNovoJogo);
            RetomarPartidaCommand = new RelayCommand(ExecutarRetomarPartida, o => PodeRetomar);
            VerRegrasCommand = new RelayCommand(ExecutarVerRegras);
            ConsultarEstatisticasCommand = new RelayCommand(ExecutarConsultarEstatisticas);
        }

        private void ExecutarCriarNovoJogo(object obj)
        {
            var jogo = new Jogo();
            string userName = Environment.UserName;
            jogo.Jogadores.Add(new Jogador { Nome = userName, IsBot = false });

            for (int i = 1; i <= NumeroBots; i++)
            {
                jogo.Jogadores.Add(new Jogador { Nome = $"Bot {i}", IsBot = true });
            }

            jogo.Mesa.Baralho = BaralhoFactory.GerarBaralhoOficial();

            foreach (var jogador in jogo.Jogadores)
            {
                for (int i = 0; i < 7; i++)
                {
                    jogador.Cartas.Add(jogo.Mesa.Baralho.Cartas[0]);
                    jogo.Mesa.Baralho.Cartas.RemoveAt(0);
                }
            }

            int idxCartaTopo = 0;
            while (jogo.Mesa.Baralho.Cartas[idxCartaTopo].Cor == "Preto")
            {
                idxCartaTopo++;
            }
            var cartaTopo = jogo.Mesa.Baralho.Cartas[idxCartaTopo];
            jogo.Mesa.Baralho.Cartas.RemoveAt(idxCartaTopo);
            jogo.Mesa.CartasJogadas.Add(cartaTopo);

            _mainViewModel.NavegarParaTabuleiro(jogo);
        }

        private void ExecutarRetomarPartida(object obj)
        {
            _mainViewModel.NavegarParaSaves();
        }

        private void ExecutarVerRegras(object obj)
        {
            _mainViewModel.NavegarParaRegras();
        }

        private void ExecutarConsultarEstatisticas(object obj)
        {

        }
    }
}