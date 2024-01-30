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


        // GET: /movie/mongo or movie/mysql
        [HttpGet("{database}")]
        public async Task<List<MovieModel>> Get(string database)
        {
            return await _movieService.GetAsync(database);
        }

        // GET movie by id: movie/mongo/65250242a1fc1c92c2d11459
        // GET movie by id: movie/mysql/1
        [HttpGet("{database}/{id}")]
        public async Task<ActionResult<MovieModel>> Get(string database, string id)
        {
            var movie = await _movieService.GetAsync(database, id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }

        // POST: movie/mongo
        // POST: movie/mysql
        [HttpPost("{database}")]
        public async Task<ActionResult<MovieModel>> Post(string database, MovieModel newMovie)
        {
            var response = await _movieService.CreateAsync(database, newMovie);
            if (database == "mongo")
            {
                return CreatedAtAction(nameof(Get), new { id = newMovie.Id }, newMovie);
            }
            else
            {
                return CreatedAtAction(nameof(Get), new { id = response }, newMovie);
            }
        }

        // PUT: movie/mongo/65250242a1fc1c92c2d11459
        // PUT: movie/mysql/1
        [HttpPut("{database}/{id}")]
        public async Task<ActionResult> Put(string database, string id, MovieModel updateMovie)
        {
            MovieModel ifExists = await _movieService.GetAsync(database, id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _movieService.UpdateAsync(database, id, updateMovie);
            return Ok();
        }

        // DELETE: movie/mongo/65250242a1fc1c92c2d11459
        // DELETE: movie/mysql/1
        [HttpDelete("{database}/{id}")]
        public async Task<ActionResult> Delete(string database, string id)
        {
            MovieModel ifExists = await _movieService.GetAsync(database, id);
            if (ifExists == null)
            {
                return NotFound();
            }
            await _movieService.DeleteAsync(database, id);
            return Ok();
        }
    }
}
