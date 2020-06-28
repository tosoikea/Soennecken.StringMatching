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

        public void Init(IEquatable<T>[] pattern)
        {
            _pattern = pattern;
            m = _pattern.Length;

            _rule.Init(_pattern);
            InitDelta2();
        }

        public void Start(IEquatable<T>[] word)
        {
            _word = word;
            n = _word.Length;

            i = 0;
            // Backward comparison
            k = m - 1;
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
                int z = k - (m - 1);
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
            int j = 0;
            delta2 = new int[m];

            // Initialize with best case scenario
            for (;j < m; j++)
                delta2[j] = m;

            // 1. Rightmost occurence
            int[] next = new int[m + 1];
            next[0] = -1;
            int t = next[0];

            for (int h = 1; h <= m; h++)
            {
                while (t >= 0 && !_pattern[m - t - 1].Equals(_pattern[m - h]))
                {
                    //As soon as edge is interrupted and can no longer be extended, we store it inside delta2 (strong good suffix)
                    delta2[m - t - 1] = Math.Min(delta2[m - t - 1], h - t - 1);
                    t = next[t];
                }
                t++;
                next[h] = t;
            }

            j = 0;
            for (int rl = next[m]; rl >= 0; rl = next[rl])
            {
                int shift = m - rl;
                while (j < shift)
                {
                    delta2[j] = Math.Min(delta2[j], shift);
                    j++;
                }
            }
        }
        public BoyerMooreMatcher(IBadCharacterRule<T> rule)
        {
            this._rule = rule;
        }
    }
}
