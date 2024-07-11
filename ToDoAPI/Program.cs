using Services;
using Repositories;
using Entities;
using ToDoAPI.Attributes;
using Microsoft.AspNetCore.Identity;
using ToDoAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.DataProtection;
namespace ToDoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var AllowAll = "AllowAll";
            var builder = WebApplication.CreateBuilder(args);
            
            IConfiguration configuration = builder.Configuration;
            var key = Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"]);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowAll,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true, // Validate who created the token (issuer)
                            ValidateAudience = true, // Validate who receives the token (audience)
                            ValidateLifetime = true, // Validate token expiration
                            ValidateIssuerSigningKey = true, // Validate the token signature using a secret key
                            IssuerSigningKey = new SymmetricSecurityKey(key), // Replace with your secret key retrieval logic
                            ValidIssuer = "ToDoServer", // Replace with your issuer string
                            ValidAudience = "ToDoApp"  // Replace with your audience string
                        };
                    });
            builder.Services.AddScoped<IToDoService, ToDoService>();
            builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            app.UseCors(AllowAll);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<JwtMiddleware>();
            
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
