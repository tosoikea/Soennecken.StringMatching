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

        public void Init(IEquatable<T>[] pattern)
        {
            _pattern = pattern;
            m = _pattern.Length;

            _rule.Init(_pattern);
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
                int x = Math.Max(k - _rule.Offset(_word[i+k], k),1);
                int z = k - (m - 1);

                // Shift
                i += x;
                k -= z;

                summary.Matched = false;
                summary.Shift = new Shift()
                {
                    Strategy = Strategy.BadCharacter,
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
        public BoyerMooreHorspoolMatcher(IBadCharacterRule<T> rule)
        {
            this._rule = rule;
        }
    }
}
