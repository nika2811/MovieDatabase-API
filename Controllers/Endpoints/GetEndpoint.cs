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
        var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
        if (movie == null) return NotFound();
        return movie;
    }
}