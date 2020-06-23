using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class BoyerMooreHorspoolMatcher<T> : IMatcher<T>
    {
        private readonly IBadCharacterRule<T> _rule;
        private IEquatable<T>[] _pattern, _word;
        private int m, n, i, k;

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
        }

        public StepSummary Step()
        {
            var summary = new StepSummary()
            {
                IsFinished = true
            };

            if (i > n - m || k < 0)
                return summary;

            if (_pattern[k].Equals(_word[i + k]))
            {
                k--;
            }
            else
            {
                int x = i + (k - _rule.Offset(_word[i+k], k));
                int z = m - 1 - k;
                i = x;
                k = k - z;

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
        public BoyerMooreHorspoolMatcher(IBadCharacterRule<T> rule)
        {
            this._rule = rule;
        }
    }
}
