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
        [HttpGet]
        public async Task<List<SongModel>> Get()
        {
            return await _songService.GetAsync();
        }

        // GET song by id: /song:65250242a1fc1c92c2d11459
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SongModel>> Get(string id)
        {
            var song = await _songService.GetAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            return song;
        }

        // POST: song
        [HttpPost]
        public async Task<ActionResult<SongModel>> Post(SongModel newSong)
        {
            await _songService.CreateAsync(newSong);
            return CreatedAtAction(nameof(Get), new { id = newSong.Id}, newSong);
        }

        // PUT: song/65250242a1fc1c92c2d11459
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Put(string id, SongModel updateSong)
        {
            SongModel ifExists = await _songService.GetAsync(id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _songService.UpdateAsync(id, updateSong);
            return Ok();
        }

        // DELETE: song/65250242a1fc1c92c2d11459
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            SongModel ifExists = await _songService.GetAsync(id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _songService.DeleteAsync(id);   
            return Ok();
        }
    }
}
