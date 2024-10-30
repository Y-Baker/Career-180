using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models;

public class Author
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [Column(TypeName = "date")]
    public DateTime HiringDate { get; set; }

    [MaxLength (100)]
    [Required]
    public string Username { get; set; }

    [MaxLength (100)]
    [Required]
    public string Password { get; set; }

    [MaxLength (250)]
    public string? bref { get; set; }

    public virtual List<News> News { get; set; } = new List<News>();
}
