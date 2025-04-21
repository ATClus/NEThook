using NEThook.Presentation.WebHook.Routes;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.RegisterEvent();

app.Run();
