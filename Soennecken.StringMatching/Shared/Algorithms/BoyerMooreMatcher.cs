using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class BoyerMooreMatcher<T> : IMatcher <T>
    {
        private readonly IBadCharacterRule<T> _rule;
        private IEquatable<T>[] _pattern, _word;
        private int m, n, i, k;

        private int[] delta2;

        public void Init(IEquatable<T>[] pattern, IEquatable<T>[] word)
        {
            _pattern = pattern;
            _word = word;

            m = _pattern.Length;
            n = _word.Length;

            i = 0;
            // Backward comparison
            k = m - 1;

            _rule.Init(_pattern);
            InitDelta2();
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

            if (i > n - m || k < 0)
                return summary;

            if (_pattern[k].Equals(_word[i + k]))
            {
                k--;
            }
            else
            {
                // Bad character rule
                int bcx = k - _rule.Offset(_word[i + k], k);
                int sgsx = delta2[k];
                int z = k - (m-1);
                int x = Math.Max(bcx, sgsx);

                // Shift
                i += x;
                k -= z;

                summary.Matched = false;
                summary.Shift = new Shift()
                {
                    Strategy = (sgsx > bcx) ? Strategy.StrongGoodSuffix : Strategy.BadCharacter,
                    IsLeft = true,
                    X = x,
                    Z = z,
                    K = k,
                    I = i
                };
            }

            summary.IsFinished = false;
            return summary;
        }

        private void InitDelta2()
        {
            delta2 = new int[_pattern.Length];

            // Initialize with best case scenario
            for (int j = 0; j < _pattern.Length; j++)
                delta2[j] = _pattern.Length;

            // 1. Rightmost occurence
            int[] next = new int[_pattern.Length + 1];
            next[0] = -1;
            int t = next[0];

            for (int j = 1; j <= _pattern.Length; j++)
            {
                while (t >= 0 && !_pattern[_pattern.Length - t - 1].Equals(_pattern[_pattern.Length - j]))
                {
                    //As soon as edge is interrupted and can no longer be extended, we store it inside delta2 (strong good suffix)
                    delta2[_pattern.Length - t - 1] = Math.Min(delta2[_pattern.Length - t - 1], j - t - 1);
                    t = next[t];
                }
                t++;
                next[j] = t;
            }

            int z = 0;
            for (t = next[_pattern.Length]; t >= 0; t = next[t])
            {
                int shift = _pattern.Length - t;
                while (z < shift)
                {
                    delta2[z] = Math.Min(delta2[z], shift);
                    z++;
                }
            }
        }

        public BoyerMooreMatcher(IBadCharacterRule<T> rule)
        {
            this._rule = rule;
        }
    }
}
