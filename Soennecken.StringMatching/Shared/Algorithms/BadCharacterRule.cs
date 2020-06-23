using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class BadCharacterRule<T> : IBadCharacterRule<T>
    {
        private Dictionary<IEquatable<T>, int> delta1 = new Dictionary<IEquatable<T>, int>();

        public void Init(IEquatable<T>[] p)
        {
            for (int k = 0; k < p.Length; k++)
                delta1[p[k]] = k;
        }

        public int Offset(IEquatable<T> bad, int k) => delta1.GetValueOrDefault(bad, -1);
    }
}
