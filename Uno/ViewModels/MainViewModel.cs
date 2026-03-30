using System;
using System.Linq;
using System.Windows.Input;
using Uno.Models;
using Uno.Services;
using Uno.Commands;

namespace Uno.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private Jogo _jogoAtual;
        public Jogo JogoAtual
        {
            get => _jogoAtual;
            set => SetProperty(ref _jogoAtual, value);
        }

        // Comandos que a Interface Gráfica (View) vai invocar
        public ICommand IniciarPartidaCommand { get; }
        public ICommand JogarCartaCommand { get; }

        public MainViewModel()
        {
            _jogoAtual = new Jogo();
            IniciarPartidaCommand = new RelayCommand(IniciarPartida);

            JogarCartaCommand = new RelayCommand(JogarCarta);
        }

        private void IniciarPartida(object? parameter)
        {
            // --- NOVIDADE: Usar o Gestor de Dados ---
            var gestorDados = new GestorDadosService();
            var perfisGuardados = gestorDados.CarregarPerfis();

            JogoAtual.Jogadores.Clear();
            string nomeHumano = Environment.UserName;

            // Tenta encontrar o Humano no XML, se não encontrar, cria um novo
            var humano = perfisGuardados.FirstOrDefault(p => p.Nome == nomeHumano)
                         ?? new Jogador { Nome = nomeHumano, IsBot = false };

            // Tenta encontrar os Bots no XML, se não encontrar, cria-os
            var bot1 = perfisGuardados.FirstOrDefault(p => p.Nome == "Bot Alpha") ?? new Jogador { Nome = "Bot Alpha", IsBot = true };
            var bot2 = perfisGuardados.FirstOrDefault(p => p.Nome == "Bot Beta") ?? new Jogador { Nome = "Bot Beta", IsBot = true };
            var bot3 = perfisGuardados.FirstOrDefault(p => p.Nome == "Bot Gamma") ?? new Jogador { Nome = "Bot Gamma", IsBot = true };

            JogoAtual.Jogadores.Add(humano);
            JogoAtual.Jogadores.Add(bot1);
            JogoAtual.Jogadores.Add(bot2);
            JogoAtual.Jogadores.Add(bot3);

            // Incrementa o número de partidas jogadas para todos e GUARDA logo no XML!
            foreach (var j in JogoAtual.Jogadores)
            {
                j.N_Partidas_Jogadas++;
            }
            gestorDados.GuardarPerfis(JogoAtual.Jogadores);
            // ----------------------------------------

            // 2. Gerar e baralhar o baralho oficial
            var baralhoService = new BaralhoService();
            JogoAtual.Mesa.Baralho = baralhoService.CriarBaralhoCompleto();
            JogoAtual.Mesa.CartasJogadas.Clear();

            // 3. Distribuir 7 cartas a cada jogador
            foreach (var jogador in JogoAtual.Jogadores)
            {
                jogador.Cartas.Clear();
                for (int i = 0; i < 7; i++)
                {
                    jogador.Cartas.Add(JogoAtual.Mesa.Baralho.Cartas.Pop());
                }
            }

            // 4. Virar a primeira carta para a mesa
            Carta primeiraCarta = JogoAtual.Mesa.Baralho.Cartas.Pop();
            JogoAtual.Mesa.CartasJogadas.Add(primeiraCarta);

            // 5. Definir quem começa a jogar
            JogoAtual.JogadorAtivo = humano;

            // Forçar a UI a atualizar
            OnPropertyChanged(nameof(JogoAtual));
        }
        private void JogarCarta(object? parameter)
        {
            // O parameter é a Carta que clicámos na interface
            if (parameter is Carta cartaClicada)
            {
                // 1. Verifica se é a vez do humano jogar
                if (JogoAtual.JogadorAtivo == null || JogoAtual.JogadorAtivo.IsBot)
                    return; // Ignora o clique se não for a tua vez

                // 2. Obtém a carta que está visível no topo da mesa
                Carta cartaTopo = JogoAtual.Mesa.CartasJogadas[0];

                // 3. Validação das Regras do UNO: Mesma Cor, Mesmo Símbolo, ou Carta Preta (Curinga)
                bool jogadaValida = cartaClicada.Cor == cartaTopo.Cor ||
                                    cartaClicada.Simbolo == cartaTopo.Simbolo ||
                                    cartaClicada.Cor == CorCarta.Preto;

                if (jogadaValida)
                {
                    // Remove a carta da mão do jogador
                    JogoAtual.JogadorAtivo.Cartas.Remove(cartaClicada);

                    // Insere a carta no índice 0 (topo) da pilha de descartes da mesa
                    JogoAtual.Mesa.CartasJogadas.Insert(0, cartaClicada);

                    // Avança para o próximo jogador
                    AvancarTurno();
                }
            }
        }
        private void AvancarTurno()
        {
            // Descobre o índice do jogador atual na lista
            int indexAtual = JogoAtual.Jogadores.IndexOf(JogoAtual.JogadorAtivo!);
            int proximoIndex;

            // Calcula quem é o próximo baseado no sentido do jogo
            if (JogoAtual.SentidoHorario)
            {
                proximoIndex = (indexAtual + 1) % JogoAtual.Jogadores.Count;
            }
            else
            {
                proximoIndex = (indexAtual - 1 + JogoAtual.Jogadores.Count) % JogoAtual.Jogadores.Count;
            }

            // Define o novo jogador ativo
            JogoAtual.JogadorAtivo = JogoAtual.Jogadores[proximoIndex];

            // Força a UI a perceber que o jogador ativo mudou
            OnPropertyChanged(nameof(JogoAtual));

            // NOTA DE SÉNIOR: Mais tarde, se o JogoAtual.JogadorAtivo.IsBot for true, 
            // vamos disparar aqui o timer de 2 segundos para o Bot jogar!
        }
    }
}