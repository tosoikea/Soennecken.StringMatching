using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Soennecken.StringMatching.Shared.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Soennecken.StringMatching.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _service;
        // GET: api/<LanguageController>
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _service.GetLanguages();
        }

        [HttpGet("{name}/words")]
        public async Task<IEnumerable<string>> GetWords(string name, [FromQuery] int limit)
        {
            return await _service.GetRandomWords(name, limit);
        }

        [HttpGet("{name}/sentences")]
        public async Task<IEnumerable<string>> GetSentences(string name, [FromQuery] int limit)
        {
            return await _service.GetRandomSentences(name, limit);
        }

        public LanguagesController(ILanguageService service)
        {
            this._service = service;
        }
    }
}
