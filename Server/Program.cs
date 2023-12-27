using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrganizationApi.Context;
using OrganizationApi.Dto;
using OrganizationApi.Dto.Jwt;
using OrganizationApi.Entity;
using OrganizationApi.Middleware;
using OrganizationApi.Services;
using OrganizationApi.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Identity.
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add Authentication.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
    };
});

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IImportService, ImportServiceImpl>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddTransient<AppMiddleware>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<AppMiddleware>();


// Create Admin user initially.
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var authService = serviceScope.ServiceProvider.GetRequiredService<IAuthService>();
    var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
    try
    {
        

        var register = await authService.Register(new RegisterRequestModel
        {
            Email = configuration["Auth:Email"],
            Password = configuration["Auth:Password"],
            Username = configuration["Auth:Username"]
        }, UserRoles.Admin);

        Console.WriteLine("Admin created successfully...");
        Console.WriteLine("Token: " + register.Token);

    }
    catch (Exception e)
    {
        Console.WriteLine("Admin user already created...");
        Console.WriteLine("Getting admin auth token....");

        var login = await authService.Login(new LoginRequestModel
        {
            Username = configuration["Auth:Username"],
            Password = configuration["Auth:Password"],
        });

        Console.WriteLine("Admin Token : " + login.Token);
    }
}

app.Run();