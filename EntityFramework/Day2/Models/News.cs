using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models;

public class News
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Title { get; set; }

    [MaxLength(250)]
    public string? bref { get; set; }
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }

    [ForeignKey("Catalog")]
    public int catId { get; set; }
    [ForeignKey("Author")]
    public int authId { get; set; }


    public virtual Catalog Catalog { get; set; }
    public virtual Author Author { get; set; }
}
