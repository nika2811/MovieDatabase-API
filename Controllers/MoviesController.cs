using Microsoft.AspNetCore.Mvc;
using MovieDatabase_API.Controllers.Endpoints;
using MovieDatabase_API.Db;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovieController : ControllerBase
{
    private readonly AddEndpoint _addEndpoint;
    private readonly MovieContext _context;
    private readonly DeleteEndpoint _deleteEndpoint;
    private readonly GetEndpoint _getEndpoint;
    private readonly SearchEndpoint _searchEndpoint;
    private readonly UpdateEndpoint _updateEndpoint;


    public MovieController(MovieContext context, GetEndpoint getEndpoint, SearchEndpoint searchEndpoint,
        AddEndpoint addEndpoint, UpdateEndpoint updateEndpoint, DeleteEndpoint deleteEndpoint)
    {
        _context = context;
        _getEndpoint = getEndpoint;
        _searchEndpoint = searchEndpoint;
        _addEndpoint = addEndpoint;
        _updateEndpoint = updateEndpoint;
        _deleteEndpoint = deleteEndpoint;
    }

    [HttpPost("/movie/get")]
    public ActionResult<Movie> Get(int id)
    {
        return _getEndpoint.Get(id);
    }

    [HttpPost("/movie/add")]
    public async Task<IActionResult> Add([FromBody] Movie movie)
    {
        return await _addEndpoint.Add(movie);
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search(string title = "", string description = "", string director = "",
        int releaseYear = 0, int pageIndex = 0, int pageSize = 100)
    {
        return await _searchEndpoint.Search(title, description, director, releaseYear, pageIndex, pageSize);
    }


    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Movie movie)
    {
        return await _updateEndpoint.Update(id, movie);
    }


    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _deleteEndpoint.Delete(id);
    }
}