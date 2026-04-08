using System;
using System.Collections.Generic;

namespace Uno.Models
{
    [Serializable]
    public class Jogo
    {
        public List<Jogador> Jogadores { get; set; }

        // Dictionary não é serializável nativamente em XML, usamos duas listas sincronizadas para serialização
        public List<string> PontuacoesKeys { get; set; }
        public List<int> PontuacoesValues { get; set; }

        public Jogador JogadorAtivo { get; set; }
        public Mesa Mesa { get; set; }

        public Jogo()
        {
            Jogadores = new List<Jogador>();
            PontuacoesKeys = new List<string>();
            PontuacoesValues = new List<int>();
            Mesa = new Mesa();
        }

        // Métodos auxiliares para gerir as pontuações em memória
        public void SetPontuacao(string nomeJogador, int pontos)
        {
            int index = PontuacoesKeys.IndexOf(nomeJogador);
            if (index >= 0)
                PontuacoesValues[index] = pontos;
            else
            {
                PontuacoesKeys.Add(nomeJogador);
                PontuacoesValues.Add(pontos);
            }
        }

        public int GetPontuacao(string nomeJogador)
        {
            int index = PontuacoesKeys.IndexOf(nomeJogador);
            return index >= 0 ? PontuacoesValues[index] : 0;
        }
    }
}