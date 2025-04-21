namespace NEThook.Presentation.WebHook.Routes;

public static class StsUserEventRoutes
{
    public static WebApplication RegisterEvent(this WebApplication app)
    {
        var group = app.MapGroup("/sts/user/event");

        // mapeia o endpoint POST /sts/user/event/register
        group.MapPost("/register", () => Results.Ok());

        return app;
    }
}