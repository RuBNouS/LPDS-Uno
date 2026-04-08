using System.Windows;
using System.Windows.Controls;

namespace Uno.Views // <--- Atualizado para o teu novo projeto!
{
    public partial class SelecaoCorView : Window
    {
        public string CorEscolhida { get; private set; }

        public SelecaoCorView()
        {
            InitializeComponent();
            CorEscolhida = "Vermelho"; // Cor padrão de segurança
        }

        private void BotaoCor_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button botao && botao.Content != null)
            {
                CorEscolhida = botao.Content.ToString();
                this.DialogResult = true; // Fecha a janela e indica sucesso
                this.Close();
            }
        }
    }
}