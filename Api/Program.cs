using Api.Data;
using Api.Data.TestData;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OnlineShopContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("OnlineShopContext" ?? throw new InvalidOperationException("Connection string 'OnlineShopContext' not found."))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<OnlineShopContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorApp", policy =>
    {
        policy.WithOrigins("http://localhost:57510", "http://localhost:5112", "https://localhost:7232", "http://localhost:5112")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<BackgroundOrderQueue>();
builder.Services.AddHostedService<BackgroundOrderUpdateService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<OnlineShopContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await TestDataSeeder.SeedAsync(dbContext, userManager);
}

//app.UseHttpsRedirection();
app.UseCors("BlazorApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
