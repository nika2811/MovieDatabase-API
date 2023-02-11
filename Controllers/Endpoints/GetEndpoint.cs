using Microsoft.AspNetCore.Mvc;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers.Endpoints;

public class GetEndpoint : ControllerBase
{
    private readonly MovieContext _context;

    public GetEndpoint(MovieContext context)
    {
        _context = context;
    }


    public ActionResult<Movie> Get(int id)
    {
        try
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) return NotFound();
            return movie;
        }
        catch (Exception ex)
        {
            _context.ErrorLogs.Add(new ErrorLog
                { Message = ex.Message, StackTrace = ex.StackTrace, CreationDate = DateTime.Now });
            _context.SaveChanges();
            return StatusCode(500, "An error occurred while processing the request. The error has been logged.");
        }
    }
}