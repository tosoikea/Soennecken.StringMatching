using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public interface IMatcher<T>
    {
        void Init(IEquatable<T>[] pattern, IEquatable<T>[] word);
        StepSummary Step();
    }
}
