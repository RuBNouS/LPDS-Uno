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

            // 1. ALTERAÇÃO AQUI: Em vez de "Saltar" e "Inverter", usamos símbolos
            string[] acoes = { "Ø", "⇄", "+2" };

            int idCounter = 1;

            foreach (var cor in cores)
            {
                baralho.Cartas.Add(new Carta { Cor = cor, Simbolo = "0", Pontos = 0 });

                for (int i = 1; i <= 9; i++)
                {
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = i.ToString(), Pontos = i });
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = i.ToString(), Pontos = i });
                }

                foreach (var acao in acoes)
                {
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = acao, Pontos = 20 });
                    baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = cor, Simbolo = acao, Pontos = 20 });
                }
            }

            // 2. ALTERAÇÃO AQUI: Em vez de "Wild" e "Wild +4", usamos um símbolo para o curinga e apenas "+4"
            for (int i = 0; i < 4; i++)
            {
                // Podes usar "❖", "❂" ou "🎨" para representar o curinga
                baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = "Preto", Simbolo = "W", Pontos = 50 });
                baralho.Cartas.Add(new Carta { Id = idCounter++, Cor = "Preto", Simbolo = "+4", Pontos = 50 });
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