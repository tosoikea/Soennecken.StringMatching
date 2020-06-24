using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Soennecken.StringMatching.Shared
{
    public enum Matcher
    {
        Naive,
        KMP,
        BMH,
        BM
    }

    public class MatchingRequest
    {
        [Required]
        public int WordCount { get; set; }

        [Required]
        public int SentenceCount { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Matcher[] Matcher { get; set; }
    }
}
