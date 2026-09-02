using Business.IServices;
using Business.Middleware;
using Business.Services;
using DataAccess.Dapper;
using DataAccess.Models;
using DataAccess.netCore.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<Models_Context>(opstions =>
{
    opstions.UseSqlServer(builder.Configuration.GetConnectionString("connecing"));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var cache = context.HttpContext
                .RequestServices
                .GetRequiredService<IDistributedCache>(); /// lấy cache dùng 

            var sessionId = context.Principal?
                .FindFirst("SessionId")?         //t�m sessionID trong claim
                .Value;
            var ID = context.Principal?
                .FindFirst(ClaimTypes.NameIdentifier)?         //t�m sessionID trong claim
                .Value;

            var TokenKey = $"user:{ID}:active_session";

            var redisSessionId = await cache.GetStringAsync(TokenKey);
            
            if(redisSessionId !=sessionId)
            {
                context.Fail("Session không hợp lệ");
                return;
            }    


            
        }
    };
});

builder.Services.AddStackExchangeRedisCache(option =>

{ option.Configuration = configuration["RedisCacheUrl"]; });



builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<ITokenServices, TokenServices>();
builder.Services.AddScoped<IJwt_Refresh, Jwt_Refresh>();
builder.Services.AddScoped<IProduct, Product>();
builder.Services.AddScoped<IApplicationDbConnection, ApplicationDbConnection>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<DeviceIdMiddleware>();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Storefront}/{action=Index}/{id?}");

app.Run();
