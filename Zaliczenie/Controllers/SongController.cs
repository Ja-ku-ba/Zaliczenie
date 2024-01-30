using Microsoft.AspNetCore.Mvc;
using Zaliczenie.Models;
using Zaliczenie.Services;

namespace Zaliczenie.Controllers
{
    [Route("song/")]
    [ApiController]
    public class SongController : Controller
    {
        private readonly GeneralService<SongModel> _songService;
        public SongController(GeneralService<SongModel> songService)
        {
            _songService = songService;
        }

        // GET: /song
        [HttpGet("{database}")]
        public async Task<List<SongModel>> Get(string database)
        {
            return await _songService.GetAsync(database);
        }

        // GET song by id: /song:65250242a1fc1c92c2d11459
        [HttpGet("{database}/{id}")]
        public async Task<ActionResult<SongModel>> Get(string database, string id)
        {
            var song = await _songService.GetAsync(database, id);
            if (song == null)
            {
                return NotFound();
            }
            return song;
        }

        // POST: song
        [HttpPost("{database}")]
        public async Task<ActionResult<SongModel>> Post(string database, SongModel newSong)
        {
            var response = await _songService.CreateAsync(database, newSong);
            if (database == "mongo")
            {
                return CreatedAtAction(nameof(Get), new { id = newSong.Id }, newSong);
            }
            else
            {
                return CreatedAtAction(nameof(Get), new { id = response }, newSong);
            }
        }

        // PUT: song/65250242a1fc1c92c2d11459
        [HttpPut("{database}/{id}")]
        public async Task<ActionResult> Put(string database, string id, SongModel updateSong)
        {
            SongModel ifExists = await _songService.GetAsync(database, id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _songService.UpdateAsync(database, id, updateSong);
            return Ok();
        }

        // DELETE: song/65250242a1fc1c92c2d11459
        [HttpDelete("{database}/{id}")]
        public async Task<ActionResult> Delete(string database, string id)
        {
            
            var res = await _songService.DeleteAsync(database, id);   
            if (res != null)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
