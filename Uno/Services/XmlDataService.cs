using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Uno.Models;

namespace Uno.Services
{
    public class XmlDataService
    {
        private readonly string _saveDirectory;

        public XmlDataService()
        {
            string pastaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _saveDirectory = Path.Combine(pastaDocumentos, "UnoSaves");

            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        public string GetNextSaveName()
        {
            int i = 1;
            while (File.Exists(Path.Combine(_saveDirectory, $"save{i}.xml")))
            {
                i++;
            }
            return $"save{i}";
        }

        public void SaveGame(Jogo jogo, string saveName)
        {
            if (string.IsNullOrWhiteSpace(saveName)) saveName = GetNextSaveName();

            string filePath = Path.Combine(_saveDirectory, $"{saveName}.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(Jogo));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, jogo);
            }
        }

        public Jogo LoadGame(string saveName)
        {
            string filePath = Path.Combine(_saveDirectory, $"{saveName}.xml");
            if (!File.Exists(filePath)) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(Jogo));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (Jogo)serializer.Deserialize(reader);
            }
        }

        public List<string> GetSavedGames()
        {
            if (!Directory.Exists(_saveDirectory)) return new List<string>();

            return Directory.GetFiles(_saveDirectory, "*.xml")
                            .Select(Path.GetFileNameWithoutExtension)
                            .ToList();
        }

        public bool HasSavedGame()
        {
            return GetSavedGames().Count > 0;
        }

        public void DeleteSave(string saveName)
        {
            string filePath = Path.Combine(_saveDirectory, $"{saveName}.xml");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}