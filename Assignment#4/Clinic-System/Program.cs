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
StdinService.Decorate("Info: you can use", ConsoleColor.DarkYellow, end: "");
StdinService.Decorate("`Esc`", ConsoleColor.Magenta, end: "");
StdinService.Decorate("key to get back in the program at any time and", ConsoleColor.DarkYellow, end: "");
StdinService.Decorate("`Ctrl + C`", ConsoleColor.Magenta, end: "");
StdinService.Decorate("to exit the program", ConsoleColor.DarkYellow);
Console.ReadLine();

app.Run();
