using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddDbContext<AilensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// 🔸 Bật Controllers (đặt TRƯỚC Build)
builder.Services.AddControllers();

// Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.SaveTokens = true;
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Route mặc định về /Customer/Home
app.MapGet("/", context =>
{
    context.Response.Redirect("/Customer/Home");
    return Task.CompletedTask;
});

// 🔸 Map Razor Pages & API (đặt CUỐI)
app.MapRazorPages();
app.MapControllers();

app.Run();
