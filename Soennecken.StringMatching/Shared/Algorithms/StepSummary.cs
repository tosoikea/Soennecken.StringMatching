using System;
using System.Collections.Generic;
using System.Text;

namespace Soennecken.StringMatching.Shared.Algorithms
{
    public class StepSummary
    {
        public int I { get; set; }
        public int K { get; set; }
        public bool Matched { get; set; }
        public Shift Shift {get;set;}
        public bool IsFinished { get; set; }
    }
}
