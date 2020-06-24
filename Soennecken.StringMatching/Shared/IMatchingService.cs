using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Soennecken.StringMatching.Shared
{
    public interface IMatchingService
    {
        Task<MatchingResponse> Simulate(MatchingRequest request, bool addTestData);
    }
}
