using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Uno.Models
{
    public class Jogador : ObservableObject
    {
        private string _nome = string.Empty;
        public string Nome
        {
            get => _nome;
            set => SetProperty(ref _nome, value);
        }

        private string? _fotografiaPath;
        public string? Fotografia
        {
            get => _fotografiaPath;
            set => SetProperty(ref _fotografiaPath, value);
        }

        private int _nPartidasJogadas;
        public int N_Partidas_Jogadas
        {
            get => _nPartidasJogadas;
            set => SetProperty(ref _nPartidasJogadas, value);
        }

        private int _nPartidasGanhos;
        public int N_Partidas_Ganhos
        {
            get => _nPartidasGanhos;
            set => SetProperty(ref _nPartidasGanhos, value);
        }

        private int _nJogosJogados;
        public int N_Jogos_Jogados
        {
            get => _nJogosJogados;
            set => SetProperty(ref _nJogosJogados, value);
        }

        private int _nJogosGanhos;
        public int N_Jogos_Ganhos
        {
            get => _nJogosGanhos;
            set => SetProperty(ref _nJogosGanhos, value);
        }

        // O XmlIgnore diz ao serializador para não tentar guardar a mão de cartas no perfil
        [XmlIgnore]
        public ObservableCollection<Carta> Cartas { get; } = new ObservableCollection<Carta>();

        // Mantemos um set público para o XML conseguir recriar este valor ao carregar
        public bool IsBot { get; set; }
    }
}