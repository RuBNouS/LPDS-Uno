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

		private string _mensagemErro;
		public string MensagemErro
		{
			get => _mensagemErro;
			set { _mensagemErro = value; OnPropertyChanged(); }
		}

		public ObservableCollection<SaveItemViewModel> ListaSaves { get; } = new ObservableCollection<SaveItemViewModel>();

		public ICommand CarregarSaveCommand { get; }
		public ICommand ApagarSaveCommand { get; }
		public ICommand EditarSaveCommand { get; }
		public ICommand RenomearSaveCommand { get; }
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
					ExecutarRenomearSave(saveItem);
				}
				else
				{
					saveItem.IsEditing = true;
				}
			}
		}

		private void ExecutarRenomearSave(object parametro)
		{
			if (parametro is SaveItemViewModel saveItem)
			{
				saveItem.IsEditing = false;

				string novoNome = saveItem.NomeSave?.Trim();

				if (string.IsNullOrWhiteSpace(novoNome) || novoNome == saveItem.NomeOriginal)
				{
					saveItem.NomeSave = saveItem.NomeOriginal;
					return;
				}

				bool nomeJaExiste = ListaSaves.Any(s => s != saveItem && s.NomeOriginal.Equals(novoNome, System.StringComparison.OrdinalIgnoreCase));

				if (nomeJaExiste)
				{
					MensagemErro = "Erro: Já existe um jogo guardado com esse nome!";
					saveItem.NomeSave = saveItem.NomeOriginal;
					return;
				}
				saveItem.NomeSave = novoNome;
				saveItem.NomeOriginal = saveItem.NomeSave;
			}
		}
	}

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