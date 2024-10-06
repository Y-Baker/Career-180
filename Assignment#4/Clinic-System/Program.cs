ConsoleApp app = new();
StdinService.Decorate("Info: you can use `Esc` key to get back in the program at any time and `Ctrl + C` to exit the program", ConsoleColor.DarkYellow);
Thread.Sleep(5000);
app.Run();
