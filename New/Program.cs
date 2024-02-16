using BusinessLogic;
using DataAccess;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using New;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomService();

//додаємо Репозиторій

builder.Services.AddRepository();

builder.Services.CustomMapper();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Рядок підключення
builder.Services.AddDbContext<BookingDbContext>(opt =>
opt.UseSqlServer(builder.Configuration.GetConnectionString("DataConnection")));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Stores.MaxLengthForKeys = 128;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    //options.SignIn.RequireConfirmedEmail = true;
})
                .AddEntityFrameworkStores<BookingDbContext>()
                .AddDefaultTokenProviders();

var app = builder.Build();

//Додаємо ролі у Dbcontext
app.SeedData();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
// Створення шляху до папки "images" в поточній робочій директорії.

if (!Directory.Exists(dir))
{
    // Перевірка, чи папка "images" не існує.

    Directory.CreateDirectory(dir);
    // Створення папки "images", якщо вона не існує.
}

app.UseStaticFiles(new StaticFileOptions
{
    // Конфігурація використання статичних файлів у додатку.

    FileProvider = new PhysicalFileProvider(dir),
    // Вказується, що фізичний провайдер файлів використовується для отримання файлів із заданого шляху.

    RequestPath = "/images"
    // Вказується, який URL-шлях буде використовуватися для доступу до статичних файлів (у цьому випадку - "/images").
});

app.UseRouting();

//Ініцілізуємо глобальну обробку помилок
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
