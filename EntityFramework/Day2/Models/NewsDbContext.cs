using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models;

internal class NewsDbContext : DbContext
{
    public virtual DbSet<Catalog> catalogs { get; set; }
    
    public virtual DbSet<Author> authors { get; set; }

    public virtual DbSet<News> news { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Server=.; Database=learnit;Trusted_Connection=True;TrustServerCertificate=True;";
        optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
    }
}
