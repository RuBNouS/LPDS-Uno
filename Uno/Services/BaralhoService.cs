using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Models;

namespace Uno.Services
{
    public class BaralhoService
    {
        // Método principal que gera e baralha as 108 cartas
        public Baralho CriarBaralhoCompleto()
        {
            var listaCartas = new List<Carta>();

            // 1. Gerar cartas coloridas (Vermelho, Amarelo, Verde, Azul)
            CorCarta[] cores = { CorCarta.Vermelho, CorCarta.Amarelo, CorCarta.Verde, CorCarta.Azul };

            foreach (var cor in cores)
            {
                // Carta Zero (Apenas 1 por cor)
                listaCartas.Add(CriarCarta(SimboloCarta.Zero, cor));

                // Cartas 1 a 9 (Duas por cor)
                for (int i = 1; i <= 9; i++)
                {
                    SimboloCarta simboloNumero = (SimboloCarta)i; // O Enum mapeia 0-9 diretamente
                    listaCartas.Add(CriarCarta(simboloNumero, cor));
                    listaCartas.Add(CriarCarta(simboloNumero, cor));
                }

                // Cartas de Ação (Duas por cor)
                listaCartas.Add(CriarCarta(SimboloCarta.Salta, cor));
                listaCartas.Add(CriarCarta(SimboloCarta.Salta, cor));
                listaCartas.Add(CriarCarta(SimboloCarta.Inverter, cor));
                listaCartas.Add(CriarCarta(SimboloCarta.Inverter, cor));
                listaCartas.Add(CriarCarta(SimboloCarta.Compra2, cor));
                listaCartas.Add(CriarCarta(SimboloCarta.Compra2, cor));
            }

            // 2. Gerar Curingas (4 Curingas normais e 4 Curingas +4, cor Preta)
            for (int i = 0; i < 4; i++)
            {
                listaCartas.Add(CriarCarta(SimboloCarta.Curinga, CorCarta.Preto));
                listaCartas.Add(CriarCarta(SimboloCarta.CuringaMais4, CorCarta.Preto));
            }

            // 3. Baralhar as cartas
            BaralharLista(listaCartas);

            // 4. Colocar na Stack do objeto Baralho
            var baralho = new Baralho();
            baralho.Cartas = new Stack<Carta>(listaCartas);

            return baralho;
        }

        // Método auxiliar para criar uma carta e atribuir os pontos oficiais
        private Carta CriarCarta(SimboloCarta simbolo, CorCarta cor)
        {
            int pontos = CalcularPontos(simbolo);

            return new Carta
            {
                Simbolo = simbolo,
                Cor = cor,
                Pontos = pontos
            };
        }

        // Regras oficiais de pontuação do Uno
        private int CalcularPontos(SimboloCarta simbolo)
        {
            return simbolo switch
            {
                SimboloCarta.Zero => 0,
                SimboloCarta.Um => 1,
                SimboloCarta.Dois => 2,
                SimboloCarta.Tres => 3,
                SimboloCarta.Quatro => 4,
                SimboloCarta.Cinco => 5,
                SimboloCarta.Seis => 6,
                SimboloCarta.Sete => 7,
                SimboloCarta.Oito => 8,
                SimboloCarta.Nove => 9,
                SimboloCarta.Salta or SimboloCarta.Inverter or SimboloCarta.Compra2 => 20,
                SimboloCarta.Curinga or SimboloCarta.CuringaMais4 => 50,
                _ => 0
            };
        }

        // Algoritmo de Fisher-Yates moderno para baralhar de forma aleatória e justa
        private void BaralharLista(List<Carta> cartas)
        {
            // Usamos Random.Shared que é a forma mais otimizada e segura no .NET moderno
            int n = cartas.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Shared.Next(n + 1);
                Carta value = cartas[k];
                cartas[k] = cartas[n];
                cartas[n] = value;
            }
        }
    }
}