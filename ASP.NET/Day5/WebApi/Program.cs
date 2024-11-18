using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.UnitOfWorks;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ShopDbContext>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("SQL-Server")));
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Online Shop",
        Version = "v1",
        Description = "RESTFul Api For Online Shop (Products)",
        TermsOfService = new Uri("https://github.com/Y-Baker"),
        Contact = new OpenApiContact()
        {
            Email = "yuossefbakier@gmail.com",
            Name = "Yousef Bakier"
        }
    });

    option.EnableAnnotations();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
