using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

var builder = WebApplication.CreateBuilder(args);

// ======== Services ========
builder.Services.AddRazorPages();
builder.Services.AddSession();

// ✅ Kết nối DB
builder.Services.AddDbContext<AilensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// ✅ Controllers
builder.Services.AddControllers();

// ✅ Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Auth/Login";            // Khi chưa đăng nhập
    options.AccessDeniedPath = "/Auth/AccessDenied"; // Khi không có quyền
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.SaveTokens = true;
});

builder.Services.AddAuthorization(options =>
{
    // ✅ Policy riêng cho admin
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

// ======== Middleware ========
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

// ✅ Điều hướng mặc định
app.MapGet("/", context =>
{
    context.Response.Redirect("/Customer/Home");
    return Task.CompletedTask;
});

// ✅ Map Razor Pages + Controllers
app.MapRazorPages();
app.MapControllers();

app.Run();
