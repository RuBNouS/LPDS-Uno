using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Uno.Models;

namespace Uno.Services
{
    public class GestorDadosService
    {
        private readonly string _pastaDestino;
        private readonly string _caminhoFicheiroPerfis;

        public GestorDadosService()
        {
            // Vai buscar a pasta pessoal do utilizador do Windows (ex: C:\Users\O_Teu_Nome)
            string pastaPerfil = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Cria uma subpasta para o nosso jogo
            _pastaDestino = Path.Combine(pastaPerfil, "UnoGame");
            _caminhoFicheiroPerfis = Path.Combine(_pastaDestino, "perfis_jogadores.xml");
        }

        // Método para Guardar a lista de jogadores no XML
        public void GuardarPerfis(IEnumerable<Jogador> jogadores)
        {
            // Garante que a pasta existe antes de tentar guardar
            if (!Directory.Exists(_pastaDestino))
            {
                Directory.CreateDirectory(_pastaDestino);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Jogador>));
            using (StreamWriter writer = new StreamWriter(_caminhoFicheiroPerfis))
            {
                // Converte para List para o serializer funcionar sem problemas
                serializer.Serialize(writer, jogadores.ToList());
            }
        }

        // Método para Carregar a lista de jogadores do XML
        public List<Jogador> CarregarPerfis()
        {
            // Se for a primeira vez que o jogo corre, o ficheiro não existe
            if (!File.Exists(_caminhoFicheiroPerfis))
            {
                return new List<Jogador>();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Jogador>));
                using (StreamReader reader = new StreamReader(_caminhoFicheiroPerfis))
                {
                    return (List<Jogador>)serializer.Deserialize(reader) ?? new List<Jogador>();
                }
            }
            catch
            {
                // Se o XML estiver corrompido, devolvemos uma lista vazia por segurança
                return new List<Jogador>();
            }
        }
    }
}