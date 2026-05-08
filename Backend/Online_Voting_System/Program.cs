
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Voting_System.Application.Services;
using Online_Voting_System.Data;
using Online_Voting_System.Domain.Entity;
using Scalar.AspNetCore;
using System.Text;

namespace Online_Voting_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Add OpenAPI with JWT security scheme
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, ct) =>
                {
                    document.Components ??= new OpenApiComponents();

                    // Define the Bearer security scheme
                    document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "Enter JWT token below"
                        }
                    };
                    return Task.CompletedTask;
                });

                // Target specific endpoints
                options.AddOperationTransformer((operation, context, ct) =>
                {
                    var requiresAuth = context.Description.ActionDescriptor
                        .EndpointMetadata
                        .OfType<AuthorizeAttribute>()
                        .Any();

                    if (requiresAuth)
                    {
                        operation.Security = new List<OpenApiSecurityRequirement>
                        {
                            new OpenApiSecurityRequirement
                            {
                                [new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                }] = Array.Empty<string>()
                            }
                        };
                    }

                    return Task.CompletedTask;
                });
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<Voter, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //Register jwt (3)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)), // ! ensure the key is defenitely not null
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddScoped<IAuthService, AuthService>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                // Getting services of Rolemanager(identityRole) in rolemanager(var = object/ instance)
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Seed role
                string[] roles = { "Admin", "User" };

                // Cheacking if the user exist
                foreach (var role in roles)
                {
                    // Only create if specific role doesnt exist
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "My API";
                    options.Theme = ScalarTheme.BluePlanet;
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecuritySchemes = new List<string> { "Bearer" }
                    };
                });
            }

            app.UseHttpsRedirection();

            // The Order is critical
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
