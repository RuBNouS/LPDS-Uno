using System;
using System.Collections.Generic;

namespace Uno.Models
{
    [Serializable]
    public class Mesa
    {
        public List<Carta> CartasJogadas { get; set; }
        public Baralho Baralho { get; set; }

        public Mesa()
        {
            CartasJogadas = new List<Carta>();
            Baralho = new Baralho();
        }
    }
}