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
                IsFinished = true,
                K = k,
                I = i,
                Matched = true
            };

            if (i > n - m || k==m)
                return summary;

            if (_pattern[k].Equals(_word[i + k]))
            {
                k++;
            }
            else
            {
                int x = 1;
                int z = k;

                // Shift
                i += x;
                k -= z;

                summary.Matched = false;
                summary.Shift = new Shift()
                {
                    Strategy = Strategy.Naive,
                    X = x,
                    Z = z,
                    K = k,
                    I = i
                };
            }

            summary.IsFinished = false;
            return summary;
        }
    }
}
