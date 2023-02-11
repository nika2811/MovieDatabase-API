using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers.Endpoints;

public class AddEndpoint : ControllerBase
{
    private readonly MovieContext _context;


    public AddEndpoint(MovieContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Add([FromBody] Movie movie)
    {
        if (string.IsNullOrEmpty(movie.Name)) return BadRequest("Movie name is required.");
        if (string.IsNullOrEmpty(movie.Description)) return BadRequest("Movie short description is required.");
        if (movie.ReleaseYear < 1895) return BadRequest("Release year must be at least 1895.");
        if (string.IsNullOrEmpty(movie.Director)) return BadRequest("Director is required.");

        // Create a list to hold the movie's genres
        var movieGenres = new List<MovieGenre>();

        // Iterate through the genres passed in the request
        foreach (var genre in movie.MovieGenres)
        {
            // Check if the genre already exists in the Genres table
            var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genre.Genre.Name);

            // If the genre doesn't exist, add it to the Genres table
            if (existingGenre == null)
            {
                existingGenre = new Genre { Name = genre.Genre.Name };
                _context.Genres.Add(existingGenre);
            }

            // Add the genre to the movie's list of genres
            movieGenres.Add(new MovieGenre { GenreId = existingGenre.Id, MovieId = movie.Id });
        }

        // Set the movie's genres
        movie.MovieGenres = movieGenres;

        // Set other properties
        movie.Status = MovieStatus.Active;
        movie.CreationDate = DateTime.Now;

        // Add the movie to the Movies table
        _context.Movies.Add(movie);

        try
        {
            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            // If an error occurs, log the error to the ErrorLogs table
            _context.ErrorLogs.Add(new ErrorLog
                { Message = ex.Message, StackTrace = ex.StackTrace, CreationDate = DateTime.Now });
            await _context.SaveChangesAsync();

            // Return a 500 Internal Server Error response
            return StatusCode(500, "An error occurred while processing the request. The error has been logged.");
        }
    }
}