using News.Models;
using System.Text;

namespace Day2
{
    public partial class Form1 : Form
    {
        NewsDbContext context;
        public Form1()
        {
            InitializeComponent();
            context = new NewsDbContext();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgv_news.DataSource = context.news.Select(e => new {e.Title, e.bref, e.Description, e.DateTime, CatalogName=e.Catalog.Name, AuthorName=e.Author.Name}).ToList();
        }
    }
}
