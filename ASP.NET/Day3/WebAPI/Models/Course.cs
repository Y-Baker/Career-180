using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models;

public class Course
{
    public int Id { get; set; }

    [StringLength(50)]
    public string? Crs_name { get; set; }

    [StringLength(150)]
    public string? Crs_desc { get; set; }
    public int? Duration { get; set; }
}