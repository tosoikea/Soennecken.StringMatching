using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Renci.SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Services
{
    internal class DatabaseLanguage
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Database { get; set; }
    }

    public class DatabaseLanguageService : ILanguageService
    {
        private readonly string _connectionParams;

        //TODO: CHANGE, ELSE VULNERABLE TO ATTACKS
        private const string _getSentences = "SELECT `sentence` FROM {0}.`sentences` ORDER BY RAND() LIMIT @maxCount";
        private const string _getWords = "SELECT `word` FROM {0}.`words` ORDER BY RAND() LIMIT @maxCount";
        private const string _getLanguages = "SELECT `name`, `type`, `database` FROM `Dictionary`.`Languages`";

        public async Task<string[]> GetLanguages()
        {
            return (await GetDatabaseLanguages()).Select(lng => lng.Name).ToArray();
        }

        public async Task<string[]> GetRandomSentences(string language, int maxCount)
        {
            var database = await GetDatabase(language);
            List<string> sentences = new List<string>();

            using (var connection = new MySqlConnection(_connectionParams))
            {
                connection.Open();
                var cmd = new MySqlCommand(String.Format(_getSentences, database), connection);
                cmd.Parameters.AddWithValue("@maxCount", maxCount);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        sentences.Add(reader.GetString(0));
                }
            }

            return sentences.ToArray();
        }

        public async Task<string[]> GetRandomWords(string language, int maxCount)
        {
            var database = await GetDatabase(language);
            List<string> words = new List<string>();

            using (var connection = new MySqlConnection(_connectionParams))
            {
                connection.Open();
                var cmd = new MySqlCommand(String.Format(_getWords, database), connection);
                cmd.Parameters.AddWithValue("@maxCount", maxCount);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                        words.Add(reader.GetString(0));
                }
            }

            return words.ToArray();
        }

        private async Task<string> GetDatabase(string language)
        {
            var languages = await GetDatabaseLanguages();
            //THROWS ERROR IF NOT GIVEN
            return languages.Where(lg => lg.Name.Equals(language)).Select(lg => lg.Database).First();
        }

        private async Task<List<DatabaseLanguage>> GetDatabaseLanguages()
        {
            List<DatabaseLanguage> languages = new List<DatabaseLanguage>();

            using (var connection = new MySqlConnection(_connectionParams))
            {
                connection.Open();
                var cmd = new MySqlCommand(_getLanguages, connection);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        languages.Add(
                            new DatabaseLanguage()
                            {
                                Name = reader.GetString(0),
                                Type = reader.GetString(1),
                                Database = reader.GetString(2),
                            });
                    }
                }
            }

            return languages;
        }

        public DatabaseLanguageService(IConfiguration config)
        {
            _connectionParams = config.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
        }
    }
}
