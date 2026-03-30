using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Uno.Models
{
    public class Jogo : ObservableObject
    {
        // Lista de todos os jogadores (Humano + Bots)
        public ObservableCollection<Jogador> Jogadores { get; } = new ObservableCollection<Jogador>();

        // Dicionário para as pontuações da partida
        private Dictionary<Jogador, int> _pontuacoes = new Dictionary<Jogador, int>();
        public Dictionary<Jogador, int> Pontuacoes
        {
            get => _pontuacoes;
            set => SetProperty(ref _pontuacoes, value);
        }

        // Quem é o jogador que tem de jogar agora
        private Jogador? _jogadorAtivo;
        public Jogador? JogadorAtivo
        {
            get => _jogadorAtivo;
            set => SetProperty(ref _jogadorAtivo, value);
        }

        // A Mesa que contém o baralho e as cartas já jogadas
        private Mesa _mesa = new Mesa();
        public Mesa Mesa
        {
            get => _mesa;
            set => SetProperty(ref _mesa, value);
        }

        // --- A PROPRIEDADE QUE ESTAVA EM FALTA ---
        // True = Ordem da lista (0, 1, 2...). False = Ordem inversa.
        private bool _sentidoHorario = true;
        public bool SentidoHorario
        {
            get => _sentidoHorario;
            set => SetProperty(ref _sentidoHorario, value);
        }
    }
}