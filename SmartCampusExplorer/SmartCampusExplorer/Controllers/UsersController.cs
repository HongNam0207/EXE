using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartCampusExplorer.Models;
using SmartCampusExplorer.Models.Entities;
using SmartCampusExplorer.Models.ViewModels.Auth;
using System.Security.Claims;

public class UsersController : Controller
{
    private readonly AilensContext _context;

    public UsersController(AilensContext context)
    {
        _context = context;
    }


    //Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users.FirstOrDefault(u =>
            u.Email == model.Username && u.PasswordHash == model.Password); // dùng password đơn giản để test

        if (user == null)
        {
            ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        if (user.Role == "Admin")
            return RedirectToAction("Index", "Analytics");

        if (user.Role == "User")
            return RedirectToAction("Index", "Locations");

        if (user.Role == "Guest")
            return RedirectToAction("Index", "Home");

        // Nếu không khớp role nào, về mặc định
        return RedirectToAction("Index", "Home");
    }


    //Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var exists = _context.Users.Any(u => u.Email == model.Email);
        if (exists)
        {
            ModelState.AddModelError("Email", "Email đã tồn tại.");
            return View(model);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email,
            PasswordHash = model.Password, // nên hash sau
            Role = "User",
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Login");
    }


    //ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user == null)
        {
            ModelState.AddModelError("Email", "Không tìm thấy email trong hệ thống.");
            return View(model);
        }

        // Ghi log hoặc hiển thị thông báo giả lập
        ViewBag.Message = "Một email hướng dẫn đặt lại mật khẩu đã được gửi (giả lập).";
        return View("ForgotPassword");
    }

}
