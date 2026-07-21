using System.Text;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.WebAPI.Hubs;
using OnlineAuctionApp.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineAuctionApp.Application;
using OnlineAuctionApp.Infrastructure;
using OnlineAuctionApp.Persistence;
using OnlineAuctionApp.Persistence.Seeders;
using Scalar.AspNetCore;
using OnlineAuctionApp.WebAPI.Middlewares;
using OnlineAuctionApp.WebAPI.Constants;
using OnlineAuctionApp.WebAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddOpenApi();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddScoped<IRealtimeNotificationService, RealtimeNotificationService>();
builder.Services.AddHostedService<AuctionClosingBackgroundService>();

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("JWT key is missing in configuration.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authorizationHeader = context.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader) &&
                context.Request.Cookies.TryGetValue(AuthCookieNames.AccessToken, out var accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        },

        OnChallenge = async context =>
        {
            context.HandleResponse();

            var message = string.IsNullOrWhiteSpace(context.Error)
                ? "Authentication is required."
                : "Invalid or expired authentication token.";

            var response = ResponseModelHelper
                .CreateUnauthorizedResponse<string>(message);

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        },

        OnForbidden = async context =>
        {
            var response = ResponseModelHelper
                .CreateForbiddenResponse<string>(
                    "You are not allowed to access this resource.");

            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();