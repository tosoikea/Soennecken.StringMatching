using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class BoyerMooreHorspoolSundayMatcher<T> : IMatcher <T>
    {
        private IEquatable<T>[] _pattern, _word;
        private int m, n, i, k;

        private Dictionary<IEquatable<T>, int> _delta1;

        public void Init(IEquatable<T>[] pattern)
        {
            _pattern = pattern;
            m = _pattern.Length;

            InitBadCharacterRule();
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
                // Offset always calculated from the last character in pattern - BMH
                // Character to the right of the pattern must be included in matching, if given - BMHS
                int x = m - ((i + m < n) ? Offset(_word[i + m]) : 0);
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

        /*
         * Strategy : BadCharacterRule
         */
        private void InitBadCharacterRule()
        {
            _delta1 = new Dictionary<IEquatable<T>, int>();
            // Very important : The BMH ignores the last character in the pattern for the delta1 table
            for (int k = 0; k < m - 1; k++)
                _delta1[_pattern[k]] = k;
        }

        private int Offset(IEquatable<T> bad) => _delta1.GetValueOrDefault(bad, -1);
    }
}
