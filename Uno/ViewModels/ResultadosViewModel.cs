using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class ResultadosViewModel : ViewModelBase
    {
        private readonly XmlDataService _dataService;
        private readonly MainViewModel _mainViewModel; // Para navegação

        public Jogo JogoFinalizado { get; set; }
        public Jogador Vencedor { get; set; }
        public ObservableCollection<Jogador> Classificacao { get; set; }

        public ICommand VoltarLobbyCommand { get; }
        public ICommand ProximaRondaCommand { get; }

        public ResultadosViewModel(Jogo jogo, XmlDataService dataService, MainViewModel mainViewModel)
        {
            JogoFinalizado = jogo;
            _dataService = dataService;
            _mainViewModel = mainViewModel;

            VoltarLobbyCommand = new RelayCommand(o => _mainViewModel.NavegarParaLobby());
            ProximaRondaCommand = new RelayCommand(o => IniciarNovaRonda());

            CalcularResultados();
        }

        private void CalcularResultados()
        {
            // O vencedor é quem ficou com 0 cartas
            Vencedor = JogoFinalizado.Jogadores.FirstOrDefault(j => j.Cartas.Count == 0);

            if (Vencedor != null)
            {
                // Calcular os pontos das cartas que sobraram nas mãos dos adversários
                int pontosGanhos = 0;
                foreach (var jogador in JogoFinalizado.Jogadores.Where(j => j != Vencedor))
                {
                    pontosGanhos += jogador.Cartas.Sum(c => c.Pontos);
                }

                // Atualizar pontuação no modelo de Jogo
                int pontosAtuais = JogoFinalizado.GetPontuacao(Vencedor.Nome);
                JogoFinalizado.SetPontuacao(Vencedor.Nome, pontosAtuais + pontosGanhos);

                Vencedor.N_Partidas_Ganhos++;
                foreach (var j in JogoFinalizado.Jogadores) j.N_Partidas_Jogadas++;
            }

            // Ordenar para a UI
            Classificacao = new ObservableCollection<Jogador>(
                JogoFinalizado.Jogadores.OrderByDescending(j => JogoFinalizado.GetPontuacao(j.Nome))
            );

            // Guardar estatísticas
            _dataService.SaveStats(JogoFinalizado);

            // Limpa o save do jogo suspenso, pois a partida terminou
            _dataService.DeleteSavedGame();
        }

        private void IniciarNovaRonda()
        {
            // Lógica para reiniciar o Jogo e navegar de volta para o TabuleiroViewModel
            _mainViewModel.IniciarNovoJogo(JogoFinalizado.Jogadores.Count(j => j.IsBot));
        }
    }
}