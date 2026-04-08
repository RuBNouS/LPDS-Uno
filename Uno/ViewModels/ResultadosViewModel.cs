using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uno.Models;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
    public class ResultadoJogador
    {
        public int Posicao { get; set; }
        public string Nome { get; set; }
        public int Pontos { get; set; }
        public string Iniciais => Nome?.Length >= 2 ? Nome.Substring(0, 2).ToUpper() : Nome?.ToUpper();
        public string CorFundo => Posicao == 1 ? "#FCEADE" : "#F8F9FA";
        public string CorTextoAvatar => Posicao == 1 ? "#D97757" : "#4A4A4A";
        public string CorFundoAvatar => Posicao == 1 ? "#F5C2B3" : "#E2E2E2";
    }

    public class ResultadosViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainViewModel;
        private Jogo _jogo;

        public string NomeVencedor { get; private set; }
        public ObservableCollection<ResultadoJogador> Ranking { get; private set; }

        public ICommand ProximaPartidaCommand { get; }
        public ICommand SairCommand { get; }

        // O construtor agora aceita os 3 argumentos corretamente
        public ResultadosViewModel(Jogo jogo, XmlDataService dataService, MainViewModel mainViewModel)
        {
            _jogo = jogo;
            _mainViewModel = mainViewModel;

            var resultados = _jogo.Jogadores.Select(j => new ResultadoJogador
            {
                Nome = j.Nome,
                Pontos = j.Cartas.Sum(c => c.Pontos)
            })
            .OrderBy(r => r.Pontos)
            .ToList();

            for (int i = 0; i < resultados.Count; i++)
            {
                resultados[i].Posicao = i + 1;
            }

            Ranking = new ObservableCollection<ResultadoJogador>(resultados);
            NomeVencedor = resultados.FirstOrDefault()?.Nome;

            ProximaPartidaCommand = new RelayCommand(ExecutarProximaPartida);
            SairCommand = new RelayCommand(ExecutarSair);
        }

        private void ExecutarProximaPartida(object obj)
        {
            _mainViewModel.NavegarParaLobby();
        }

        private void ExecutarSair(object obj)
        {
            _mainViewModel.NavegarParaLobby();
        }
    }
}