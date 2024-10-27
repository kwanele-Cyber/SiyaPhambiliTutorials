using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SiyaphambiliTutorials.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure services and the app's request pipeline
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Apply migrations and seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Ensure the database is updated
    await InitializeRoles(scope.ServiceProvider);
    await SeedAdminUser(scope.ServiceProvider);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

// Method to initialize roles
static async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Student", "Tutor", "Administrator" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Method to seed an administrator
static async Task SeedAdminUser(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string adminEmail = "admin@siyaphambili.com";
    string adminPassword = "SecurePassword123!";

    var user = await userManager.FindByEmailAsync(adminEmail);
    if (user == null)
    {
        user = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",  // Ensure FirstName is set
            LastName = "User",    // Ensure LastName is set, assuming it's also required
            EmailConfirmed = true
        };
        var createUser = await userManager.CreateAsync(user, adminPassword);
        if (createUser.Succeeded)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }
            await userManager.AddToRoleAsync(user, "Administrator");
        }
        else
        {
            // Log the errors or handle them as necessary
            foreach (var error in createUser.Errors)
            {
                Console.WriteLine($"Error: {error.Description}");
            }
        }
    }
}

