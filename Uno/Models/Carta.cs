using System;

namespace Uno.Models
{
    [Serializable]
    public class Carta
    {
        public int Id { get; set; }
        public string Simbolo { get; set; } // Ex: "0", "9", "Inverter", "+2", "Wild"
        public string Cor { get; set; }     // Ex: "Vermelho", "Azul", "Verde", "Amarelo", "Preto" (para Wild)
        public int Pontos { get; set; }
    }
}