using System;
using System.IO;
using System.Xml.Serialization;

namespace Uno.Services
{
    public class XmlDataService
    {
        private readonly string _basePath;
        private readonly string _gameSavePath;
        private readonly string _statsSavePath;

        public XmlDataService()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _basePath = Path.Combine(userProfile, "UnoGameData");

            // Garante que a pasta existe
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }

            _gameSavePath = Path.Combine(_basePath, "PartidaSuspensa.xml");
            _statsSavePath = Path.Combine(_basePath, "Estatisticas.xml");
        }

        public void SaveGame(Models.Jogo jogo)
        {
            SerializeToXml(jogo, _gameSavePath);
        }

        public Models.Jogo LoadGame()
        {
            return DeserializeFromXml<Models.Jogo>(_gameSavePath);
        }

        public void SaveStats(Models.Jogo estatisticas) // Reutilizamos a estrutura Jogadores para guardar estatísticas gerais
        {
            SerializeToXml(estatisticas, _statsSavePath);
        }

        public Models.Jogo LoadStats()
        {
            return DeserializeFromXml<Models.Jogo>(_statsSavePath);
        }

        public bool HasSavedGame()
        {
            return File.Exists(_gameSavePath);
        }

        public void DeleteSavedGame()
        {
            if (File.Exists(_gameSavePath))
            {
                File.Delete(_gameSavePath);
            }
        }

        private void SerializeToXml<T>(T data, string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, data);
                }
            }
            catch (Exception ex)
            {
                // Em cenário real, deveríamos fazer log do erro
                throw new Exception($"Erro ao guardar o ficheiro XML em {filePath}: {ex.Message}");
            }
        }

        private T DeserializeFromXml<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar o ficheiro XML de {filePath}: {ex.Message}");
            }
        }
    }
}