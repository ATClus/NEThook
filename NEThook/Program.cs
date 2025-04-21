using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NEThook.Presentation.WebHook.Routes;

var builder = WebApplication.CreateBuilder(args);

var jwtConfig = builder.Configuration.GetSection("AuthSettings");
var issuer       = jwtConfig["Issuer"];
var audience     = jwtConfig["Audience"];
var secret       = jwtConfig["EncryptionKey"];
var keyId = "my-dev-key-01";

if (string.IsNullOrWhiteSpace(issuer) ||
    string.IsNullOrWhiteSpace(audience) ||
    string.IsNullOrWhiteSpace(secret))
{
    throw new InvalidOperationException("Missing JWT configuration in AppSettings");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
{
    KeyId = keyId
};

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken            = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidIssuer              = issuer,
            ValidateAudience         = true,
            ValidAudience            = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = signingKey,
            ValidateLifetime         = true,
            ClockSkew                = TimeSpan.Zero,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"Authentication Failed: {ctx.Exception?.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine("Authentication Challenge invoked.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterEvent();

app.Run();
