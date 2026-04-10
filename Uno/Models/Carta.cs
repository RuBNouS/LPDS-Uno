using System;
using Uno.ViewModels.Base;

namespace Uno.Models
{
    [Serializable]
    public class Carta : ViewModelBase
    {
        public int Id { get; set; } 
        public string Simbolo { get; set; }
        public string Cor { get; set; }
        public int Pontos { get; set; }

        private bool _podeSerJogada;
        public bool PodeSerJogada
        {
            get => _podeSerJogada;
            set { _podeSerJogada = value; OnPropertyChanged(); }
        }
    }
}