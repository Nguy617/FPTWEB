using Microsoft.EntityFrameworkCore;                    // ← Bắt buộc cho UseSqlServer
using FPTPlay.Data;
using FPTPlay.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;                                     // ← Namespace của FPTPlayContext

var builder = WebApplication.CreateBuilder(args);

//format global toàn site - Tất cả date = chuẩn VN
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");

//Cấu hình Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký DbContext với SQL Server
builder.Services.AddDbContext<FPTPlayContext>(options =>
    options.UseSqlServer (
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 🔥 Đăng ký Service
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();                    // Hiển thị lỗi chi tiết khi dev
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

// 🔥 QUAN TRỌNG: phải có Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

//Đăng ký Area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();