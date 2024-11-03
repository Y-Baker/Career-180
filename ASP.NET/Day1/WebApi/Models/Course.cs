namespace WebApi.Models;

public class Course
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Duration { get; set; }
    public bool Status { get; set; }
}
