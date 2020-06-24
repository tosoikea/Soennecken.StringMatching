using Soennecken.StringMatching.Shared.Algorithms;
using Soennecken.StringMatching.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared
{
    public class BasicMatchingService : IMatchingService
    {
        private readonly ILanguageService _languageService;

        public async Task<MatchingResponse> Simulate(MatchingRequest request, bool addTestData)
        {
            string[] sentences = await _languageService.GetRandomSentences(request.Language, request.SentenceCount);
            string[] words = await _languageService.GetRandomWords(request.Language, request.WordCount);

            var response = new MatchingResponse()
            {
                Summaries = new MatchingSummary[request.Matcher.Length]
            };

            if (addTestData)
            {
                response.Sentences = sentences;
                response.Words = words;
            }

            var sentencesEq = sentences.Select((sentence) => IMatcher<char>.ConvertFromString(sentence)).ToArray();
            var wordsEq = words.Select((word) => IMatcher<char>.ConvertFromString(word)).ToArray();

            //SPAWN IN NEW THREAD AND THEN AWAIT FINISH
            for(int mI = 0; mI < response.Summaries.Length; mI++)
            {
                response.Summaries[mI] = new MatchingSummary()
                {
                    Matcher = request.Matcher[mI]
                };
                GetMatcherResponse(sentencesEq, wordsEq, response.Summaries[mI]);
            }

            return response;
        }

        private void GetMatcherResponse(IEquatable<char>[][] sentences, IEquatable<char>[][] words, MatchingSummary summary)
        {
            IMatcher<char> matcher;
            switch (summary.Matcher)
            {
                case Matcher.KMP:
                    matcher = new KnuthMorrisPrattMatcher<char>();
                    break;
                case Matcher.BMH:
                    matcher = new BoyerMooreHorspoolMatcher<char>(new BadCharacterRule<char>());
                    break;
                case Matcher.BM:
                    matcher = new BoyerMooreMatcher<char>(new BadCharacterRule<char>());
                    break;
                default:
                    matcher = new NaiveMatcher<char>();
                    break;
            }

            Run(sentences, words, matcher, summary);
        }

        private void Run(IEquatable<char>[][] sentences, IEquatable<char>[][] words, IMatcher<char> matcher, MatchingSummary summary)
        {
            var shifts = new List<int>();
            var comparisons = new int[sentences.Length * words.Length];

            for(int wI = 0; wI < words.Length; wI++)
            {
                matcher.Init(words[wI]);
                for(int sI = 0; sI < sentences.Length; sI++)
                {
                    int steps = 0;
                    matcher.Start(sentences[sI]);

                    for(var step = matcher.Step(); !step.IsFinished; step = matcher.Step())
                    {
                        steps++;

                        if (!step.Matched)
                            shifts.Add(step.Shift.I - step.I);
                    }

                    comparisons[wI * sentences.Length + sI] = steps;
                }
            }

            summary.AvgShiftLength = shifts.Average();
            summary.AvgSteps = comparisons.Average();
        }

        public BasicMatchingService(ILanguageService languageService)
        {
            _languageService = languageService;
        }
    }
}
