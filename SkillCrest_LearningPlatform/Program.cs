using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SkillCrest_LearningPlatform.Data;
using SkillCrest_LearningPlatform.Data.Data.Models;
using SkillCrest_LearningPlatform.Infrastructure.Repositories;
using SkillCrest_LearningPlatform.Infrastructure.Repositories.Contracts;
using SkillCrest_LearningPlatform.Services;
using SkillCrest_LearningPlatform.Services.Interfaces;

using SkillCrest_LearningPlatform.Infrastructure.ApplicationBuilderExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); 
builder.Services.AddRazorPages();

builder.Services.AddSingleton<DatabaseInitializer>();

string adminEmail = builder.Configuration.GetValue<string>("Administrator:Email")!;
string adminUsername = builder.Configuration.GetValue<string>("Administrator:Username")!;
string adminPassword = builder.Configuration.GetValue<string>("Administrator:Password")!;

string teacherEmail = builder.Configuration.GetValue<string>("Teacher:Email")!;
string teacherUsername = builder.Configuration.GetValue<string>("Teacher:Username")!;
string teacherPassword = builder.Configuration.GetValue<string>("Teacher:Password")!;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddRoles<IdentityRole<Guid>>()
               .AddSignInManager<SignInManager<ApplicationUser>>()
               .AddUserManager<UserManager<ApplicationUser>>()
               .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IManageService, ManageService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

var initializer = app.Services.GetRequiredService<DatabaseInitializer>();
initializer.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.SeedAdministrator(adminEmail, adminUsername, adminPassword);
app.SeedTeacher(teacherEmail, teacherUsername, teacherPassword);

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );



app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
