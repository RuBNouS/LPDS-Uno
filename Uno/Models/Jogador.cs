using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Uno.Models
{
    [Serializable]
    public class Jogador
    {
        public string Nome { get; set; }
        public string Fotografia { get; set; }

        [XmlIgnore] // Ignorado na serialização para evitar redundância, guardamos apenas o estado global do jogo
        public ObservableCollection<Carta> Cartas { get; set; }

        // Propriedade auxiliar para o serializador XML conseguir gravar a lista de cartas
        [XmlArray("CartasMao")]
        public Carta[] CartasSerializaveis
        {
            get { return Cartas != null ? new List<Carta>(Cartas).ToArray() : new Carta[0]; }
            set { Cartas = new ObservableCollection<Carta>(value); }
        }

        public int N_Partidas_Jogadas { get; set; }
        public int N_Partidas_Ganhos { get; set; }
        public int N_Jogos_Jogados { get; set; }
        public int N_Jogos_Ganhos { get; set; }
        public bool IsBot { get; set; }

        public Jogador()
        {
            Cartas = new ObservableCollection<Carta>();
        }

        public Jogador(bool isBot, string nomeBot = "")
        {
            IsBot = isBot;
            Nome = isBot ? nomeBot : Environment.UserName; // Nome automático via Windows Profile para Humano [cite: 147]
            Cartas = new ObservableCollection<Carta>();
            N_Partidas_Jogadas = 0;
            N_Partidas_Ganhos = 0;
            N_Jogos_Jogados = 0;
            N_Jogos_Ganhos = 0;
        }
    }
}