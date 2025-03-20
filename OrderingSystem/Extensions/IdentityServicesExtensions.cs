using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OrderingSystem.Repository.Data;
using OrderingSystem.Core.Services.Contract;
using OrderingSystem.Service;
using OrderingSystem.Core.Entities.Identity;
namespace OrderingSystem.API.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddIdentity<Customer, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience=true,
                        ValidAudience = configuration["JwtToken:Audience"],
                        ValidateIssuer=true,
                        ValidIssuer= configuration["JwtToken:Issuer"],
                        IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"])),
                        ValidateLifetime=true,
                        ClockSkew=TimeSpan.FromDays(double.Parse(configuration["JwtToken:TokenExpiry"]))
                    };
                });

            return services;
        }
    }
}
