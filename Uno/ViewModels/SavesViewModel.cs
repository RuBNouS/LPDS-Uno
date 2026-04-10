using System.Collections.ObjectModel;
using System.Windows.Input;
using Uno.Services;
using Uno.ViewModels.Base;

namespace Uno.ViewModels
{
	public class SavesViewModel : ViewModelBase
	{
		private readonly MainViewModel _mainViewModel;
		private readonly XmlDataService _dataService;

		// Agora a lista guarda objetos que controlam o estado de edição
		public ObservableCollection<SaveItemViewModel> ListaSaves { get; } = new ObservableCollection<SaveItemViewModel>();

		public ICommand CarregarSaveCommand { get; }
		public ICommand ApagarSaveCommand { get; }
		public ICommand EditarSaveCommand { get; }
		public ICommand RenomearSaveCommand { get; } // Novo comando para o Enter
		public ICommand VoltarCommand { get; }

		public SavesViewModel(MainViewModel mainViewModel, XmlDataService dataService)
		{
			_mainViewModel = mainViewModel;
			_dataService = dataService;

			CarregarSaveCommand = new RelayCommand(ExecutarCarregarSave);
			ApagarSaveCommand = new RelayCommand(ExecutarApagarSave);
			EditarSaveCommand = new RelayCommand(ExecutarEditarSave);
			RenomearSaveCommand = new RelayCommand(ExecutarRenomearSave);
			VoltarCommand = new RelayCommand(o => _mainViewModel.NavegarParaLobby());

			CarregarListaSaves();
		}

		private void CarregarListaSaves()
		{
			ListaSaves.Clear();
			var saves = _dataService.GetSavedGames();
			foreach (var save in saves)
			{
				ListaSaves.Add(new SaveItemViewModel { NomeSave = save, NomeOriginal = save, IsEditing = false });
			}
		}

		private void ExecutarCarregarSave(object parametro)
		{
			if (parametro is SaveItemViewModel saveItem)
			{
				var jogoGuardado = _dataService.LoadGame(saveItem.NomeOriginal);
				if (jogoGuardado != null)
				{
					_mainViewModel.NavegarParaTabuleiro(jogoGuardado, saveItem.NomeOriginal);
				}
			}
		}

		private void ExecutarApagarSave(object parametro)
		{
			if (parametro is SaveItemViewModel saveItem)
			{
				_dataService.DeleteSave(saveItem.NomeOriginal);
				CarregarListaSaves();
			}
		}

		private void ExecutarEditarSave(object parametro)
		{
			if (parametro is SaveItemViewModel saveItem)
			{
				if (saveItem.IsEditing)
				{
					// Se já estava em modo edição e clicou no lápis de novo, guarda as alterações
					ExecutarRenomearSave(saveItem);
				}
				else
				{
					// Ativa o modo de edição (mostra a caixa de texto)
					saveItem.IsEditing = true;
				}
			}
		}

		private void ExecutarRenomearSave(object parametro)
		{
			if (parametro is SaveItemViewModel saveItem)
			{
				saveItem.IsEditing = false; // Fecha a caixa de texto

				// Verifica se o utilizador não apagou tudo e se o nome realmente mudou
				if (!string.IsNullOrWhiteSpace(saveItem.NomeSave) && saveItem.NomeSave != saveItem.NomeOriginal)
				{
					// ATENÇÃO: Tens de ter um método RenameSave no teu XmlDataService para alterar o ficheiro no Windows!
					// _dataService.RenameSave(saveItem.NomeOriginal, saveItem.NomeSave);

					// Atualiza o nome original para refletir a mudança
					saveItem.NomeOriginal = saveItem.NomeSave;
				}
				else
				{
					// Se estiver vazio ou for igual, reverte para o nome anterior
					saveItem.NomeSave = saveItem.NomeOriginal;
				}
			}
		}
	}

	// Classe auxiliar que representa cada Save na lista
	public class SaveItemViewModel : ViewModelBase
	{
		private string _nomeSave;
		public string NomeSave
		{
			get => _nomeSave;
			set { _nomeSave = value; OnPropertyChanged(); }
		}

		private string _nomeOriginal;
		public string NomeOriginal
		{
			get => _nomeOriginal;
			set { _nomeOriginal = value; OnPropertyChanged(); }
		}

		private bool _isEditing;
		public bool IsEditing
		{
			get => _isEditing;
			set { _isEditing = value; OnPropertyChanged(); }
		}
	}
}