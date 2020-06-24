using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Services
{
    internal class CorporaContainer
    {
        public string CorpusName { get; set; }
        public string Description { get; set; }
        public int NumberOfSentences { get; set; }
        public int NumberOfTokens { get; set; }
    }

    internal class WordContainer
    {
        public int Id { get; set; }
        public string Word { get; set; }

        public int Freq { get; set; }
    }

    internal class SentenceContainer
    {
        public int Id { get; set; }
        public string Sentence { get; set; }
    }

    internal class SentencesContainer
    {
        public int Count { get; set; }
        public SentenceContainer[] Sentences { get; set; }
    }

    public class APILanguageService : ILanguageService
    {
        private readonly HttpClient _client;

        public async Task<string[]> GetLanguages()
        {
            CorporaContainer[] corporas = new CorporaContainer[0];
            HttpResponseMessage response = await _client.GetAsync($"/corpora/availableCorpora");

            if (response.IsSuccessStatusCode)
            {
                corporas = JsonConvert.DeserializeObject<CorporaContainer[]>(await response.Content.ReadAsStringAsync());
            }

            return corporas.Select((corpora) => corpora.CorpusName).ToArray();
        }

        public async Task<string[]> GetRandomWords(string language, int maxCount)
        {
            WordContainer[] words = new WordContainer[0];
            HttpResponseMessage response = await _client.GetAsync($"/words/{language}/randomword/?limit={maxCount}");

            if (response.IsSuccessStatusCode)
            {
                words = JsonConvert.DeserializeObject<WordContainer[]>(await response.Content.ReadAsStringAsync());
            }

            return words.Select((container) => container.Word).ToArray();
        }

        public async Task<string[]> GetRandomSentences(string language, int maxCount)
        {
            var sentences = await _PseudoRandomSentences(language, maxCount);
            return sentences.ToArray();
        }

        private async Task<IEnumerable<string>> _PseudoRandomSentences(string language, int maxCount)
        {
            List<string> sentences = new List<string>();
            string[] words = await GetRandomWords(language, maxCount);
            var random = new Random();

            for(int wI = 0; wI < words.Length && sentences.Count < maxCount; wI++)
            {
                SentencesContainer container = null;
                /*
                 * Initial information about sentences by given word
                 */
                var response = await _client.GetAsync($"/sentences/{language}/sentences/{words[wI]}?limit={1}");

                if (response.IsSuccessStatusCode)
                    container = JsonConvert.DeserializeObject<SentencesContainer>(await response.Content.ReadAsStringAsync());

                if (container == null)
                    continue;

                /*
                 * Offset sentences
                */
                int randomOffset = random.Next(0, Math.Max(0, container.Count - maxCount));
                response = await _client.GetAsync($"/sentences/{language}/sentences/{words[wI]}?limit={maxCount}?offset={randomOffset}");

                if (response.IsSuccessStatusCode)
                    container = JsonConvert.DeserializeObject<SentencesContainer>(await response.Content.ReadAsStringAsync());

                if (container == null)
                    continue;

                for (int sI = 0; sI < container.Sentences.Length && sentences.Count < maxCount; sI++)
                    sentences.Add(container.Sentences[sI].Sentence);
            }

            return sentences;
        }


        public APILanguageService(HttpClient client)
        {
            this._client = client;
        }
    }
}
