using System.Collections.Generic;

namespace Uno.Models
{
    public class Baralho : ObservableObject
    {
        private Stack<Carta> _cartas = new Stack<Carta>();

        // Esta é a propriedade "Cartas" que o BaralhoService está a tentar aceder!
        public Stack<Carta> Cartas
        {
            get => _cartas;
            set
            {
                SetProperty(ref _cartas, value);
                OnPropertyChanged(nameof(NumeroCartasRestantes));
            }
        }

        public int NumeroCartasRestantes => _cartas.Count;
    }
}