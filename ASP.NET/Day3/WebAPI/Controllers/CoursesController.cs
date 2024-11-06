using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        CourseDBContext db;
        public CoursesController(CourseDBContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult get()
        {
            if (db.Courses.Any())
                return Ok(db.Courses.ToList());
            return NotFound();
        }

        [HttpGet("{id:int}")]
        public IActionResult getById(int id)
        {
            Course? s = db.Courses.Where(n => n.Id == id).SingleOrDefault();
            if (s == null)
                return NotFound();
            else
                return Ok(s);
        }

        [HttpGet("{name:alpha}")]
        public IActionResult couseByName(string name)
        {
            Course? s = db.Courses.Where(n => n.Crs_name == name).SingleOrDefault();
            if (s == null)
                return NotFound();
            else
                return Ok(s);
        }

        [HttpDelete("{id}")]
        public IActionResult deleteCourse(int id)
        {
            Course? course = db.Courses.Where(e => e.Id == id).SingleOrDefault();
            if (course == null) return NotFound();
            db.Courses.Remove(course);
            db.SaveChanges();
            return Ok(db.Courses.ToList());
        }

        [HttpPut("{id}")]
        public IActionResult put(int id, Course course)
        {
            if (course == null || course.Id != id)
                return BadRequest();
            if (!db.Courses.Any(c => c.Id == id))
                return BadRequest();
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();

                return NoContent();
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public IActionResult post(Course course)
        {
            if (course == null) return BadRequest();
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();

                //return Created($"api/courses/{course.Id}", course);
                return CreatedAtAction("getById", new { id = course.Id }, course);
            }
            return BadRequest(ModelState);
        }
    }
}
