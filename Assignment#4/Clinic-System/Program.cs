using Microsoft.Extensions.Configuration;

// Load configuration
string? basePath = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
IConfiguration? config = null;
if (basePath is not null)
{
     config = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
}

IConfigurationSection? admin = config?.GetSection("Owner");
if (admin is null || !admin.GetChildren().Any())
    throw new Exception("Admin configuration section is empty");

Doctor doctor;
try
{
    doctor = new Doctor(admin["username"]!,
                       admin["password"]!,
                       admin["name"],
                       admin["email"]!,
                       admin["phone"],
                       (Department)Enum.Parse(typeof(Department), admin["department"]!),
                       null,
                       (Auth)Enum.Parse(typeof(Auth), admin["auth"]!)
                   );
}
catch (Exception ex)
{
    throw new Exception("Something wrong with admin configrations in appsettings.json - " + ex.Message);
}

ConsoleApp app = new(doctor, config);

StdinService.Decorate("Info: you can use", ConsoleColor.DarkYellow, end: "");
StdinService.Decorate("`Esc`", ConsoleColor.Magenta, end: "");
StdinService.Decorate("key to get back in the program at any time and", ConsoleColor.DarkYellow, end: "");
StdinService.Decorate("`Ctrl + C`", ConsoleColor.Magenta, end: "");
StdinService.Decorate("to exit the program", ConsoleColor.DarkYellow);
Console.ReadLine();

app.Run();
