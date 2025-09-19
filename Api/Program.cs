using Api.Configuration;
using Api.Data;
using Api.Repositories;
using Api.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.Configure<PasswordConfiguration>(builder.Configuration.GetSection("PasswordConfiguration"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.Name = "OnlineShopAuth";
        options.Cookie.Domain = "localhost";
        options.LoginPath = "/api/auth/login";
        options.AccessDeniedPath = "/api/auth/unauthorized";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
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

builder.Services.AddScoped<RepositoryBase>();
builder.Services.AddScoped<TaxRateRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<BasketRepository>();
builder.Services.AddScoped<BasketItemRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<WishlistRepository>();
builder.Services.AddScoped<OrderTrackingUpdateRepository>();
builder.Services.AddScoped<OrderItemRepository>();
builder.Services.AddScoped<OrderRepository>();

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
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseBuilder>>();
    await new DatabaseBuilder(logger).SetupDatabase();
}

//app.UseHttpsRedirection();
app.UseCors("BlazorApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
