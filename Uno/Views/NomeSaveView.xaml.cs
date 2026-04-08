using System.Windows;
using Uno.Services;

namespace Uno.Views
{
    public partial class NomeSaveView : Window
    {
        private readonly XmlDataService _dataService;
        public string NomeSaveEscolhido { get; private set; }

        public NomeSaveView(string defaultName, XmlDataService dataService = null)
        {
            InitializeComponent();
            _dataService = dataService ?? new XmlDataService();
            TxtNomeSave.Text = defaultName;
            TxtNomeSave.SelectAll();
            TxtNomeSave.Focus();
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string nomeInserido = TxtNomeSave.Text.Trim();
            
            if (string.IsNullOrEmpty(nomeInserido))
            {
                MensagemErro.Text = "O nome não pode estar vazio.";
                return;
            }

            // Verificar se o ficheiro já existe
            if (_dataService.GetSavedGames().Contains(nomeInserido))
            {
                MensagemErro.Text = "Já existe um save com este nome. Escolha outro ou o save será substituído.";
                return;
            }

            NomeSaveEscolhido = nomeInserido;
            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}