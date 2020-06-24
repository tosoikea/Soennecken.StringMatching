using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public interface IMatcher<T>
    {
        void Init(IEquatable<T>[] pattern);
        void Start(IEquatable<T>[] word);
        StepSummary Step();

        static IEquatable<char>[] ConvertFromString(string data)
        {
            var result = new IEquatable<char>[data.Length];

            for (int i = 0; i < data.Length; i++)
                result[i] = data[i];

            return result;
        }
    }
}
