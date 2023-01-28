using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly MovieContext _context;

    public MovieController(MovieContext context)
    {
        _context = context;
    }


    [HttpGet("get/{id}")]
    public ActionResult<Movie> Get(int id)
    {
        var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
        if (movie == null) return NotFound();
        return movie;
    }

    [HttpPost("/movie/add")]
    public async Task<IActionResult> Add([FromBody] Movie movie)
    {
        if (string.IsNullOrEmpty(movie.Name)) return BadRequest("Movie name is required.");
        if (string.IsNullOrEmpty(movie.Description)) return BadRequest("Movie short description is required.");
        if (movie.ReleaseYear < 1895) return BadRequest("Release year must be at least 1895.");
        if (string.IsNullOrEmpty(movie.Director)) return BadRequest("Director is required.");
        movie.Status = MovieStatus.Active;
        movie.CreationDate = DateTime.Now;
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        return Ok();
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search(string title = "", string description = "", string director = "",
        int releaseYear = 0, int pageIndex = 0, int pageSize = 100)
    {
        var movies = _context.Movies.Where(x => x.Status == MovieStatus.Active);

        if (!string.IsNullOrEmpty(title)) movies = movies.Where(x => x.Name.Contains(title));

        if (!string.IsNullOrEmpty(description)) movies = movies.Where(x => x.Description.Contains(description));

        if (!string.IsNullOrEmpty(director)) movies = movies.Where(x => x.Director.Contains(director));

        if (releaseYear != 0) movies = movies.Where(x => x.ReleaseYear == releaseYear);

        //return Ok(await movies.ToListAsync());
        return Ok(await movies.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());
    }


    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Movie movie)
    {
        var targetMovie = await _context.Movies.FindAsync(id);

        if (targetMovie == null) return NotFound();

        if (!string.IsNullOrEmpty(movie.Name)) targetMovie.Name = movie.Name;

        if (!string.IsNullOrEmpty(movie.Description)) targetMovie.Description = movie.Description;

        if (!string.IsNullOrEmpty(movie.Director)) targetMovie.Director = movie.Director;

        if (movie.ReleaseYear != 0) targetMovie.ReleaseYear = movie.ReleaseYear;

        _context.Movies.Update(targetMovie);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null) return NotFound();

        movie.Status = MovieStatus.Deleted;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}