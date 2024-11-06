using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    static private List<News> News = new List<News>() {
        new News { Id = 1, Title = "News 1", Description = "Description 1", Author = new Author { Id = 1, Name = "Author 1", Bref = "Bref 1", HiringDate = new DateTime(2024,1,1)} },
        new News { Id = 2, Title = "News 2", Description = "Description 2", Author = new Author { Id = 2, Name = "Author 2", Bref = "Bref 2", HiringDate = new DateTime(2022, 6, 30)} },
        new News { Id = 3, Title = "News 3", Description = "Description 3", Author = new Author { Id = 3, Name = "Author 3", Bref = "Bref 3", HiringDate = new DateTime(2021,10,10)} }
    };

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(News);
    }

    [HttpGet("{title}")]
    public IActionResult GetByTitle(string title)
    {
        News? one = News.FirstOrDefault(n => n.Title == title);
        if (one == null)
        {
            return NotFound();
        }
        return Ok(one);
    }

    [HttpGet("author/{name}")]
    public IActionResult GetByAuthor(string name)
    {
        List<News> news = News.Where(n => n.Author?.Name == name).ToList();
        if (news.Count == 0)
        {
            return NotFound();
        }
        return Ok(news);
    }

    [HttpPost]
    public IActionResult Add(News news)
    {
        if (news == null)
            return BadRequest();

        News.Add(news);
        return CreatedAtAction(nameof(GetByTitle), new { title = news.Title }, news);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, News news)
    {
        if (news == null || id != news.Id)
            return BadRequest();

        News? one = News.FirstOrDefault(n => n.Id == id);
        if (one == null)
            return NotFound();

        one.Title = news.Title;
        one.Description = news.Description;
        one.Author = news.Author;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        News? one = News.FirstOrDefault(n => n.Id == id);
        if (one == null)
            return NotFound();

        News.Remove(one);
        return Ok(one);
    }
}
