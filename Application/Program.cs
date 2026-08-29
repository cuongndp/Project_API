using Business.IServices;
using Business.Middleware;
using Business.Services;
using DataAccess.Dapper;
using DataAccess.Models;
using DataAccess.netCore.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            var db = context.HttpContext
                .RequestServices
                .GetRequiredService<Models_Context>(); /// l?y models_Context g�n d� bi?n _contexxt c� s? d?ng

            var sessionId = context.Principal?
                .FindFirst("SessionId")?         //t�m sessionID trong claim
                .Value;

            //if (!int.TryParse(sessionId, out int id))
            //{
            //    context.Fail("Invalid session");                      
            //    return;
            //}

            //chuy?n session th�nh int

            var id= Convert.ToInt32 (sessionId);

            var session = await db.dh_UserSession
                .AsNoTracking()   // n� ch? l?y d? li?u ?? ??c 
                .FirstOrDefaultAsync(x => x.ID == id);

            if (session == null)
            {
                context.Fail("Session not found");
                return;
            }

            if (session.IsRevoked)
            {
                context.Fail("Session revoked");
                return;
            }

            if (session.ExpiryDate <= DateTime.UtcNow)
            {
                context.Fail("Session expired");
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
