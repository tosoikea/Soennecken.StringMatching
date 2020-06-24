using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Soennecken.StringMatching.Shared
{
    public class MatchingSummary
    {
        [Required]
        public Matcher Matcher { get; set; }

        [Required]
        public double AvgSteps { get; set; }

        [Required]
        public double AvgShiftLength { get; set; }

    }

    public class MatchingResponse
    {
        [Required]
        public MatchingSummary[] Summaries { get; set; }

        public string[] Sentences { get; set; }

        public string[] Words { get; set; }
    }
}
