using Microsoft.EntityFrameworkCore;
using MovieDatabase_API.Models;

namespace MovieDatabase_API.Db;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
}