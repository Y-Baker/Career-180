using Microsoft.Extensions.Configuration;

// Load configuration
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

IConfigurationSection admin = config.GetSection("Owner");
Doctor doctor = new(admin["username"]!,
                    admin["password"]!,
                    admin["name"]!,
                    admin["email"]!,
                    admin["phone"]!,
                    (Department)Enum.Parse(typeof(Department), admin["department"]!),
                    null,
                    (Auth)Enum.Parse(typeof(Auth), admin["auth"]!)
                );

ConsoleApp app = new(doctor, config);
StdinService.Decorate("Info: you can use `Esc` key to get back in the program at any time and `Ctrl + C` to exit the program", ConsoleColor.DarkYellow);
Console.ReadLine();

app.Run();
