using System.Windows;
using System.Windows.Controls;

namespace Uno.Views
{
    public partial class SelecaoCorView : Window
    {
        public string CorEscolhida { get; private set; }

        public SelecaoCorView()
        {
            InitializeComponent();
            CorEscolhida = "Vermelho"; // Cor de segurança padrão
        }

        private void BotaoCor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button botao)
            {
                CorEscolhida = botao.Content.ToString();
                this.DialogResult = true; // Fecha a janela e indica sucesso
                this.Close();
            }
        }
    }
}