using System.Text;
using BlogpostService.Infrastructure;
using BlogpostService.Properties.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlogpostService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            e => e.Key,
                            e => e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                        );

                    var response = new ApiErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version())
            ));

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]!))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.Headers["Token_expired"] = "true";
                            return context.Response.WriteAsync("{\"error\": \"token_expired\"}");
                        }
                        
                        return Task.CompletedTask;
                    }
                };
            });

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.MapControllers();
        app.Run();
    }
}