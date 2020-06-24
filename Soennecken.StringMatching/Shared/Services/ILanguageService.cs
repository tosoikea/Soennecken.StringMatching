using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Services
{
    public interface ILanguageService
    {
        Task<string[]> GetLanguages();
        Task<string[]> GetRandomWords(string language, int maxCount);
        Task<string[]> GetRandomSentences(string language, int maxCount);
    }
}
