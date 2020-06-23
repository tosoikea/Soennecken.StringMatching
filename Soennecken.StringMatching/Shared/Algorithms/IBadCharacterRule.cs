using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public interface IBadCharacterRule<T>
    {
        void Init(IEquatable<T>[] p);
        int Offset(IEquatable<T> bad, int k);
    }
}
