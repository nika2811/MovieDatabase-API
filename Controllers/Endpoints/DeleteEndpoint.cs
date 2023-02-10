using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers.Endpoints;

public class DeleteEndpoint : ControllerBase
{
    private readonly MovieContext _context;

    public DeleteEndpoint(MovieContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();

        try
        {
            movie.Status = MovieStatus.Active;
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