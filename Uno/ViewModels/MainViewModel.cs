using System.Linq;
using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Esta propriedade diz à MainWindow qual é a View que deve ser mostrada no ecrã
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        private readonly XmlDataService _dataService;

        public MainViewModel()
        {
            _dataService = new XmlDataService();
            // Assim que a aplicação arranca, mandamos o utilizador para o Lobby
            NavegarParaLobby();
        }

        // --- MÉTODOS DE NAVEGAÇÃO ---

        public void NavegarParaLobby()
        {
            CurrentViewModel = new LobbyViewModel(this, _dataService);
        }

        public void NavegarParaTabuleiro(Jogo jogo)
        {
            CurrentViewModel = new TabuleiroViewModel(jogo, _dataService, this);
        }

        public void NavegarParaResultados(Jogo jogoTerminado)
        {
            CurrentViewModel = new ResultadosViewModel(jogoTerminado, _dataService, this);
        }

        // --- LÓGICA DE CRIAÇÃO DO JOGO ---

        public void IniciarNovoJogo(int numeroBots)
        {
            var novoJogo = new Jogo();

            // 1. Gera o baralho com as 108 cartas
            novoJogo.Mesa.Baralho = BaralhoFactory.GerarBaralhoOficial();

            // 2. Adiciona o jogador Humano
            var humano = new Jogador(isBot: false);
            novoJogo.Jogadores.Add(humano);

            // 3. Adiciona os Bots escolhidos
            for (int i = 1; i <= numeroBots; i++)
            {
                novoJogo.Jogadores.Add(new Jogador(isBot: true, nomeBot: $"Bot {i}"));
            }

            // 4. Distribui 7 cartas a cada jogador
            foreach (var jogador in novoJogo.Jogadores)
            {
                for (int i = 0; i < 7; i++)
                {
                    jogador.Cartas.Add(novoJogo.Mesa.Baralho.Cartas[0]);
                    novoJogo.Mesa.Baralho.Cartas.RemoveAt(0);
                }
            }

            // 5. Coloca a primeira carta na mesa (garantindo que não é uma carta preta/Wild para não complicar o início)
            var primeiraCarta = novoJogo.Mesa.Baralho.Cartas.First(c => c.Cor != "Preto");
            novoJogo.Mesa.Baralho.Cartas.Remove(primeiraCarta);
            novoJogo.Mesa.CartasJogadas.Add(primeiraCarta);

            // 6. Arranca para o ecrã do jogo!
            NavegarParaTabuleiro(novoJogo);
        }
    }
}