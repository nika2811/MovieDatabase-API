namespace MovieDatabase_API.Models;

public class MovieSearchParameters
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Director { get; set; }
    public int? ReleaseYear { get; set; }
}