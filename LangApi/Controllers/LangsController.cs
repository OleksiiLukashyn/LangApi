using System.Collections.Generic;
using System.Linq;
using LangApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LangApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LangsController : ControllerBase
    {
        private readonly List<Lang> _langs;

        public LangsController(List<Lang> langs)
        {
            _langs = langs;
        }

        // GET: langs
        [HttpGet]
        public IEnumerable<Lang> Get()
        {
            return _langs;
        }

        // GET langs/cpp
        [HttpGet("{id}")]
        public ActionResult<Lang> Get(string id)
        {
            var lang = _langs.SingleOrDefault(l => l.Id == id);
            if (lang == null)
                return NotFound();
            return lang;
        }

        // POST: langs
        [HttpPost]
        public ActionResult Post([FromForm] Lang lang)
        {
            if (ModelState.IsValid)
            {
                _langs.Add(lang);
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // PUT: langs/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromForm] Lang lang)
        {
            var oldLang = _langs.SingleOrDefault(l => l.Id == id);
            if (oldLang == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                oldLang.Id = lang.Id;
                oldLang.Year = lang.Year;
                return Ok();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var lang = _langs.SingleOrDefault(l => l.Id == id);
            if (lang == null)
                return NotFound();

            _langs.Remove(lang);
            return Ok();
        }
    }
}