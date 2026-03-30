using System;
using Uno.Models;

namespace Uno.Models
{
    public class Carta : ObservableObject
    {
        public Guid Id { get; } = Guid.NewGuid();
        public SimboloCarta Simbolo { get; init; }
        public CorCarta Cor { get; init; }
        public int Pontos { get; init; }
    }
}