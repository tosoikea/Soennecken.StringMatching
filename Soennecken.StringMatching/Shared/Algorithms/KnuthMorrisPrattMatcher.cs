using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class KnuthMorrisPrattMatcher<T> : IMatcher<T>
    {
        private IEquatable<T>[] _pattern, _word;
        private int m, n, i, k;
        private int[] next;

        public void Init(IEquatable<T>[] pattern)
        {
            _pattern = pattern;
            m = _pattern.Length;

            // Calculate border table
            PreProcess();
        }

        public void Start(IEquatable<T>[] word)
        {
            _word = word;
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

            if (i > n - m || k == m)
                return summary;

            if (_pattern[k].Equals(_word[i + k]))
            {
                k++;
            }
            else
            {
                int x = i + (k - next[k]);
                int z = k - Math.Max(0, next[k]);
                i = x;
                k = k - z;

                summary.Matched = false;
                summary.Shift = new Shift()
                {
                    Strategy = Strategy.KMP,
                    X = x,
                    Z = z,
                    K = k,
                    I = i
                };
            }

            summary.IsFinished = false;
            return summary;
        }

        private void PreProcess()
        {
            next = new int[m];
            next[0] = -1;
            int t = next[0];

            for (int k = 1; k < m; k++)
            {
                while (t >= 0 && !_pattern[k - 1].Equals(_pattern[t]))
                    t = next[t];
                t++;
                next[k] = t;
            }
        }
    }
}
