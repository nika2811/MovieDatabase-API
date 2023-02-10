namespace MovieDatabase_API.Models;

public class ErrorLog
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public DateTime CreationDate { get; set; }
}