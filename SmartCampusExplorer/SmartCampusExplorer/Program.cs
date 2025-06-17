using SmartCampusExplorer.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SmartCampusExplorer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DB
            builder.Services.AddDbContext<AilensContext>();
            builder.Services.AddScoped(typeof(AilensContext));

            // ✅ Add Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Users/Login";              // Đường dẫn khi chưa đăng nhập
                    options.LogoutPath = "/Users/Logout";            // Đường dẫn đăng xuất
                    options.AccessDeniedPath = "/Home/AccessDenied"; // Khi không đủ quyền truy cập
                });

            builder.Services.AddAuthorization(); // Cho phép sử dụng [Authorize]

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ✅ Kích hoạt xác thực và phân quyền
            app.UseAuthentication(); // Phải đặt trước UseAuthorization
            app.UseAuthorization();

            // Mặc định điều hướng
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
