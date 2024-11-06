using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public class CourseDBContext : DbContext
{
    public CourseDBContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

}
