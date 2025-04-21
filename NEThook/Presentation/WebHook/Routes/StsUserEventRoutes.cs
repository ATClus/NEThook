namespace NEThook.Presentation.WebHook.Routes;

public static class StsUserEventRoutes
{
    public static WebApplication RegisterEvent(this WebApplication app)
    {
        var group = app.MapGroup("/sts/user/event");

        group.MapPost("/register", () => Results.Ok());

        return app;
    }
}