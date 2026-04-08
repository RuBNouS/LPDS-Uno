using Uno.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using Uno.Models;

namespace Uno.Services
{
    public static class BaralhoFactory
    {
        public static Baralho GerarBaralhoOficial()
        {
            var baralho = new Baralho();
            string[] cores = { "Vermelho", "Azul", "Verde", "Amarelo" };
            string[] acoes = { "Saltar", "Inverter", "+2" };
            int idCounter = 1;

            foreach (var cor in cores)
            {
                // Um '0' por cor
                baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = "0", Pontos = 0 });

                // Dois de cada número de 1 a 9
                for (int i = 1; i <= 9; i++)
                {
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = i.ToString(), Pontos = i });
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = i.ToString(), Pontos = i });
                }

                // Duas cartas de cada ação por cor
                foreach (var acao in acoes)
                {
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = acao, Pontos = 20 });
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = acao, Pontos = 20 });
                }
            }

            // 4 Coringas (Wild) e 4 Coringas +4 (Wild +4)
            for (int i = 0; i < 4; i++)
            {
                baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = "Preto", Simbolo = "Wild", Pontos = 50 });
                baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = "Preto", Simbolo = "Wild +4", Pontos = 50 });
            }

            baralho.Cartas = Baralhar(baralho.Cartas);
            return baralho;
        }

        private static List<Carta> Baralhar(List<Carta> cartas)
        {
            var rng = new Random();
            return cartas.OrderBy(c => rng.Next()).ToList();
        }
    }
}