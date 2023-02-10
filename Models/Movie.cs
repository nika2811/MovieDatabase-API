using System.ComponentModel.DataAnnotations;

namespace MovieDatabase_API.Models;

public enum MovieStatus
{
    Active,
    Deleted
}

public class Movie
{
    public int Id { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
    public string Name { get; set; }

    [Required]
    [StringLength(2000, ErrorMessage = "Short description cannot be longer than 2000 characters.")]
    public string Description { get; set; }

    [Required] public int ReleaseYear { get; set; }

    [Required] public string Director { get; set; }

    [Required] public MovieStatus Status { get; set; }

    [Required] public DateTime CreationDate { get; set; }
    public List<MovieGenre> MovieGenres { get; set; }
}