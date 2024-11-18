using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.UnitOfWorks;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

const string corsPolicy = "AllowALl";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

builder.Services.AddCors(e => e.AddPolicy(corsPolicy, p =>
{
    p.AllowAnyOrigin();
    p.AllowAnyMethod();
    p.AllowAnyHeader();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(corsPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
