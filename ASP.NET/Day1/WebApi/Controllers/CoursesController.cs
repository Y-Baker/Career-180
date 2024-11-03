using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    static private List<Course> courses = new List<Course>()
    {
        new() {Id=1, Name="Course1", Duration=10, Status=true},
        new() {Id=2, Name="Course2", Duration=20, Status=false},
        new() {Id=3, Name="Course3", Duration=30, Status=true},
        new() {Id=4, Name="Course4", Duration=40, Status=false},
        new() {Id=5, Name="Course5", Duration=50, Status=true}
    };


    [HttpGet]
    public List<Course> GetCourses()
    {
        return courses;
    }

    [HttpGet("{id}")]
    public Course? GetCourse(int id)
    {
        return courses.Find(e => e.Id == id);
    }

    //[HttpGet("{name}")]
    //public List<Course> GetCourseByName(string name)
    //{
    //    return courses.Where(e => e.Name == name).ToList();
    //}
}
