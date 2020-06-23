using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public enum Strategy
    {
        Naive,
        KMP,
        StrongGoodSuffix,
        BadCharacter
    }

    public class Shift
    {
        public Strategy Strategy { get; internal set; }

        public bool IsLeft { get; internal set; }
        public int X { get; internal set; }
        public int Z { get; internal set; }
        public int I { get; internal set; }
        public int K { get; internal set; }
    }
}
