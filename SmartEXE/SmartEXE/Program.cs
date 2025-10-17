using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddRazorPages();
builder.Services.AddSession(); // Thêm session để lưu role, username, v.v.

// ✅ Kết nối DB
builder.Services.AddDbContext<AilensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// Thêm Authentication
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

// ✅ Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();       // Phải đặt sau UseRouting và trước MapRazorPages

app.UseAuthentication();
app.UseAuthorization();

// ✅ Cấu hình route mặc định: mở trang /Customer/Home
app.MapGet("/", context =>
{
    context.Response.Redirect("/Customer/Home");
    return Task.CompletedTask;
});

// ✅ Map Razor Pages
app.MapRazorPages();

// ✅ Chạy ứng dụng
app.Run();
