using backend.Core.JwtOp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend.TokenGeneration{
    public static class AddApi
    {
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>   
                {       
                    options.RequireHttpsMetadata = true;    
                    options.SaveToken = true;   
                    
                    options.TokenValidationParameters = new TokenValidationParameters       
                    {          
                        ValidateIssuer = false,
                        ValidateAudience = false,         
                        ValidateLifetime = true,          
                        ValidateIssuerSigningKey = true,       
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SekretKey))       
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["test-Cookies"];
                            return Task.CompletedTask;
                        }
                    };
                });
                      
        }
    }
}
