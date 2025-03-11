
using System.Text;
using Business.DTO;
using Business.Interfaces;
using Business.Profiles;
using Business.Services;
using Business.Validators;
using DataAccess;
using DataAccess.Repository.Service;
using DataAccess.Repository.Services;
using Events.Adapters;
using Events.MiddleWares;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Events
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<EventRepository>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<PlaceRepository>();
            builder.Services.AddScoped<SubscriptionRepository>();

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<HashService>();
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<SubService>();

            builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql("Host=postgresdb;Database=events;Username=events;Password=events"));

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAutoMapper(typeof(UserProfile));
            builder.Services.AddAutoMapper(typeof(EventProfile));

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddScoped<IValidator<RegisterDTO>, RegisterValidator>();
            builder.Services.AddScoped<IValidator<LoginDTO>, LoginValidator>();
            builder.Services.AddScoped<IValidator<EventDTO>, EventValidator>();
            builder.Services.AddScoped<IWebRootPath, WebRootAdapter>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseCors(builder =>
            {
                builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
