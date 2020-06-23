using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Soennecken.StringMatching.Shared
{
   public class MatchingRequest
    {
        [Required]
        public string Word { get; set; }
        [Required]
        public string Pattern { get; set; }
    }
}
