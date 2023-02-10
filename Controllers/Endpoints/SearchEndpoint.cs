using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers.Endpoints;

public class SearchEndpoint : ControllerBase
{
    private readonly MovieContext _context;


    public SearchEndpoint(MovieContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Search(string title = "", string description = "", string director = "",
        int releaseYear = 0, int pageIndex = 0, int pageSize = 100)
    {
        var movies = _context.Movies.Where(x => x.Status == MovieStatus.Active);

        if (!string.IsNullOrEmpty(title)) movies = movies.Where(x => x.Name.Contains(title));

        if (!string.IsNullOrEmpty(description)) movies = movies.Where(x => x.Description.Contains(description));

        if (!string.IsNullOrEmpty(director)) movies = movies.Where(x => x.Director.Contains(director));

        if (releaseYear != 0) movies = movies.Where(x => x.ReleaseYear == releaseYear);

        return Ok(await movies.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync());
    }
}