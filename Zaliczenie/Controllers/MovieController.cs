using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zaliczenie.Models;
using Zaliczenie.Services;

namespace Zaliczenie.Controllers
{
    [Route("movie/")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly GeneralService<MovieModel> _movieService;

        public MovieController(GeneralService<MovieModel> movieService)
        {
            _movieService = movieService;
        }


        // GET: /movie
        [HttpGet]
        public async Task<List<MovieModel>> Get()
        {
            return await _movieService.GetAsync();
        }

        // GET movie by id: /movie:65250242a1fc1c92c2d11459
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<MovieModel>> Get(string id)
        {
            var movie = await _movieService.GetAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        // POST: movie
        [HttpPost]
        public async Task<ActionResult<MovieModel>> Post(MovieModel newMovie)
        {
            await _movieService.CreateAsync(newMovie);
            return CreatedAtAction(nameof(Get), new { id = newMovie.Id }, newMovie);
        }

        // PUT: movie/65250242a1fc1c92c2d11459
        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Put(string id, MovieModel updateMovie)
        {
            MovieModel ifExists = await _movieService.GetAsync(id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _movieService.UpdateAsync(id, updateMovie);
            return Ok();
        }

        // DELETE: movie/65250242a1fc1c92c2d11459
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            MovieModel ifExists = await _movieService.GetAsync(id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _movieService.DeleteAsync(id);
            return Ok();
        }
    }
}
