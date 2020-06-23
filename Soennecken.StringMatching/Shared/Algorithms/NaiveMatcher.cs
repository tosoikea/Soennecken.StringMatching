using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class NaiveMatcher<T> : IMatcher<T>
    {
        private IEquatable<T>[] _pattern, _word;
        private int m, n, i, k;

        public void Init(IEquatable<T>[] pattern, IEquatable<T>[] word)
        {
            _pattern = pattern;
            _word = word;

            m = _pattern.Length;
            n = _word.Length;

            i = 0;
            k = 0;
        }

        public StepSummary Step()
        {
            var summary = new StepSummary()
            {
                IsFinished = true
            };

            if (i > n - m || k==m)
                return summary;

            if (_pattern[k].Equals(_word[i + k]))
            {
                k++;
            }
            else
            {
                i = i + 1;
                k = 0;

                summary.Shift = new Shift()
                {
                    Strategy = Strategy.Naive,
                    X = 1,
                    Z = -k,
                    K = 0,
                    I = i
                };
            }

            summary.IsFinished = false;
            return summary;
        }
    }
}
