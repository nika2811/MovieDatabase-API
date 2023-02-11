using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers.Endpoints;

public class UpdateEndpoint : ControllerBase
{
    private readonly MovieContext _context;


    public UpdateEndpoint(MovieContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Update(int id, [FromBody] Movie movie)
    {
        if (id != movie.Id) return BadRequest("Ids do not match.");

        if (string.IsNullOrEmpty(movie.Name)) return BadRequest("Movie name is required.");
        if (string.IsNullOrEmpty(movie.Description)) return BadRequest("Movie short description is required.");
        if (movie.ReleaseYear < 1895) return BadRequest("Release year must be at least 1895.");
        if (string.IsNullOrEmpty(movie.Director)) return BadRequest("Director is required.");

        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie == null) return NotFound("Movie not found.");


        existingMovie.Name = movie.Name;
        existingMovie.Description = movie.Description;
        existingMovie.Director = movie.Director;
        existingMovie.ReleaseYear = movie.ReleaseYear;

        var movieGenres = new List<MovieGenre>();

        foreach (var genre in movie.MovieGenres)
        {
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genre.Genre.Name);

            if (existingGenre == null)
            {
                existingGenre = new Genre { Name = genre.Genre.Name };
                _context.Genres.Add(existingGenre);
            }

            movieGenres.Add(new MovieGenre { GenreId = existingGenre.Id, MovieId = movie.Id });
        }

        existingMovie.MovieGenres = movieGenres;

        _context.Movies.Update(existingMovie);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // If an error occurs, log the error to the ErrorLogs table
            _context.ErrorLogs.Add(new ErrorLog
                { Message = ex.Message, StackTrace = ex.StackTrace, CreationDate = DateTime.Now });

            // Return a 500 Internal Server Error response
            return StatusCode(500, "An error occurred while updating the movie.");
        }

        return Ok();
    }
}