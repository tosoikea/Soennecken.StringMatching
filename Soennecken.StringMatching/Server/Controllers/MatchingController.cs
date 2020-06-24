using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Soennecken.StringMatching.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Soennecken.StringMatching.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _service;
        // POST api/<MatchingController>
        [HttpPost]
        public async Task<MatchingResponse> Post([FromBody] MatchingRequest request, [FromQuery] bool addTestData)
        {
            var response = await _service.Simulate(request, addTestData);
            return response;
        }

        public MatchingController(IMatchingService service)
        {
            _service = service;
        }
    }
}
