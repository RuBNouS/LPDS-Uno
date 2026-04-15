using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Uno.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string cor)
            {
                // Mapeia o texto da cor para as cores bonitas dos mockups   
                return cor switch
                {
                    "Vermelho" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E63946")),
                    "Azul" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#457B9D")),
                    "Verde" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A9D8F")),
                    "Amarelo" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E9C46A")),
                    "Preto" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")), // Para cartas Especiais/Wild
                    _ => new SolidColorBrush(Colors.LightGray) // Cor de segurança
                };
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Não precisamos de converter da cor de volta para string neste jogo
            throw new NotImplementedException();
        }
    }
}