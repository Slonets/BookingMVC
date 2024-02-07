using BusinessLogic;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using New;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomService();

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

    options.SignIn.RequireConfirmedEmail = true;
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

app.UseRouting();

//Ініцілізуємо глобальну обробку помилок
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
