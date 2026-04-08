using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class TabuleiroViewModel : ViewModelBase
    {
        private readonly XmlDataService _dataService;
        private readonly MainViewModel _mainViewModel; // Variável adicionada
        private Jogo _jogoAtual;
        private bool _isHumanTurn;
        private bool _unoGritadoPeloHumano;
        private int _sentidoJogo = 1; // 1 para a direita, -1 para a esquerda
        private int _indiceJogadorAtual;

        // Propriedades para Bindings na UI
        public Jogo JogoAtual
        {
            get => _jogoAtual;
            set { _jogoAtual = value; OnPropertyChanged(); }
        }

        public Carta CartaTopo => JogoAtual?.Mesa?.CartasJogadas?.LastOrDefault();

        public bool IsHumanTurn
        {
            get => _isHumanTurn;
            set { _isHumanTurn = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Carta> MaoHumano => JogoAtual?.Jogadores.FirstOrDefault(j => !j.IsBot)?.Cartas;

        // Comandos (Ações dos botões)
        public ICommand JogarCartaCommand { get; }
        public ICommand ComprarCartaCommand { get; }
        public ICommand GritarUnoCommand { get; }
        public ICommand SuspenderPartidaCommand { get; }

        // Construtor corrigido com os 3 argumentos
        public TabuleiroViewModel(Jogo jogo, XmlDataService dataService, MainViewModel mainViewModel)
        {
            _jogoAtual = jogo;
            _dataService = dataService;
            _mainViewModel = mainViewModel; // Guardamos a referência

            JogarCartaCommand = new RelayCommand(ExecutarJogarCarta, PodeJogarCarta);
            ComprarCartaCommand = new RelayCommand(ExecutarComprarCarta, o => IsHumanTurn);
            GritarUnoCommand = new RelayCommand(o => _unoGritadoPeloHumano = true, o => IsHumanTurn && MaoHumano.Count == 2);
            SuspenderPartidaCommand = new RelayCommand(ExecutarSuspenderPartida);

            IniciarJogo();
        }

        private void IniciarJogo()
        {
            _indiceJogadorAtual = 0;
            ProcessarTurno();
        }

        private async void ProcessarTurno()
        {
            JogoAtual.JogadorAtivo = JogoAtual.Jogadores[_indiceJogadorAtual];
            OnPropertyChanged(nameof(JogoAtual));

            if (!JogoAtual.JogadorAtivo.IsBot)
            {
                IsHumanTurn = true;
                _unoGritadoPeloHumano = false; // Reset para o turno
            }
            else
            {
                IsHumanTurn = false;
                await RotinaBotAsync(JogoAtual.JogadorAtivo);
            }
        }

        private bool PodeJogarCarta(object parametro)
        {
            if (!IsHumanTurn || parametro is not Carta carta) return false;

            // Regra UNO Oficial: Cores iguais, Símbolos iguais ou cartas pretas (Wild)
            return carta.Cor == CartaTopo.Cor || carta.Simbolo == CartaTopo.Simbolo || carta.Cor == "Preto";
        }

        private void ExecutarJogarCarta(object parametro)
        {
            if (parametro is Carta carta)
            {
                AplicarJogada(JogoAtual.JogadorAtivo, carta);
            }
        }

        private void ExecutarComprarCarta(object parametro)
        {
            ComprarCartas(JogoAtual.JogadorAtivo, 1);
            AvancarTurno();
        }

        private void AplicarJogada(Jogador jogador, Carta carta)
        {
            // Validação de penalização por esquecer o UNO
            if (!jogador.IsBot && jogador.Cartas.Count == 2 && !_unoGritadoPeloHumano)
            {
                ComprarCartas(jogador, 2);
            }

            jogador.Cartas.Remove(carta);
            JogoAtual.Mesa.CartasJogadas.Add(carta);

            // SE A CARTA FOR PRETA E FOR O HUMANO A JOGAR
            if (carta.Cor == "Preto" && !jogador.IsBot)
            {
                var popup = new Views.SelecaoCorView();
                if (popup.ShowDialog() == true)
                {
                    // Muda a cor "virtual" da carta na mesa para a cor escolhida!
                    CartaTopo.Cor = popup.CorEscolhida;
                }
            }
            // SE FOR O BOT A JOGAR, ELE ESCOLHE UMA COR ALEATÓRIA (IA Simples)
            else if (carta.Cor == "Preto" && jogador.IsBot)
            {
                string[] cores = { "Vermelho", "Azul", "Verde", "Amarelo" };
                CartaTopo.Cor = cores[new Random().Next(0, 4)];
            }

            OnPropertyChanged(nameof(CartaTopo));

            // Verificar Fim de Jogo
            if (jogador.Cartas.Count == 0)
            {
                FinalizarPartida(jogador);
                return;
            }

            AplicarEfeitoCartaEspecial(carta);
            AvancarTurno();
        }

        private void AplicarEfeitoCartaEspecial(Carta carta)
        {
            switch (carta.Simbolo)
            {
                case "Inverter":
                    _sentidoJogo *= -1;
                    break;
                case "Saltar":
                    CalcularProximoIndice();
                    break;
                case "+2":
                    var alvo = JogoAtual.Jogadores[ProximoIndice()];
                    ComprarCartas(alvo, 2);
                    CalcularProximoIndice();
                    break;
            }
        }

        private async Task RotinaBotAsync(Jogador bot)
        {
            await Task.Delay(2000);

            var cartasValidas = bot.Cartas.Where(c => c.Cor == CartaTopo.Cor || c.Simbolo == CartaTopo.Simbolo || c.Cor == "Preto").ToList();

            if (cartasValidas.Any())
            {
                var cartaEscolhida = cartasValidas.OrderByDescending(c => c.Pontos).FirstOrDefault();
                AplicarJogada(bot, cartaEscolhida);
            }
            else
            {
                ComprarCartas(bot, 1);
                AvancarTurno();
            }
        }

        private void ComprarCartas(Jogador jogador, int quantidade)
        {
            for (int i = 0; i < quantidade; i++)
            {
                if (JogoAtual.Mesa.Baralho.Cartas.Any())
                {
                    var cartaComprada = JogoAtual.Mesa.Baralho.Cartas[0];
                    JogoAtual.Mesa.Baralho.Cartas.RemoveAt(0);
                    jogador.Cartas.Add(cartaComprada);
                }
            }
        }

        private void AvancarTurno()
        {
            CalcularProximoIndice();
            ProcessarTurno();
        }

        private void CalcularProximoIndice()
        {
            _indiceJogadorAtual = ProximoIndice();
        }

        private int ProximoIndice()
        {
            int max = JogoAtual.Jogadores.Count;
            int next = (_indiceJogadorAtual + _sentidoJogo) % max;
            if (next < 0) next += max;
            return next;
        }

        private void ExecutarSuspenderPartida(object parametro)
        {
            _dataService.SaveGame(JogoAtual);
            _mainViewModel.NavegarParaLobby(); // Navega de volta ao menu principal
        }

        private void FinalizarPartida(Jogador vencedor)
        {
            _mainViewModel.NavegarParaResultados(JogoAtual); // Navega para o ecrã de resultados
        }
    }
}