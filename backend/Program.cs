using backend.Context;
using Microsoft.EntityFrameworkCore;
using backend.Core.JwtOp;
using Microsoft.AspNetCore.CookiePolicy;
using backend.TokenGeneration;
using backend.Core.AutoMapper;


var builder = WebApplication.CreateBuilder(args);
//Create services to the container.
var service = builder.Services;
var configuration = builder.Configuration;

service.AddControllers();
//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();

service.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseNpgsql(configuration.GetConnectionString("Care"));
    });

service.AddControllers();
   
service.AddAuthorization();
service.AddApiAuthentication(configuration);
service.AddAutoMapper(typeof(AutoMapperConfigProfile));

service.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option =>
{
    option
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader();
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
