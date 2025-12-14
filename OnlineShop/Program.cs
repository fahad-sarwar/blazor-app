using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using OnlineShopUI.Components;
using OnlineShopUI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddHttpClient("Api", client =>
    {
        client.BaseAddress = new Uri("http://localhost:5110/");
    });

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddSingleton<CustomAuthenticationStateService>();
builder.Services.AddScoped<CheckoutService>();
builder.Services.AddScoped<ServiceBase>();
builder.Services.AddScoped<BasketService>();
builder.Services.AddScoped<AnonymousUserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BasketCountService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<WishlistService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
