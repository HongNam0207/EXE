using Microsoft.EntityFrameworkCore;
using SmartEXE.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddRazorPages();
builder.Services.AddSession(); // Thêm session để lưu role, username, v.v.

// ✅ Kết nối DB
builder.Services.AddDbContext<AilensContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

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
