using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChinaCatSunflower.Data;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Data.Common;
using ChinaCatSunflower.AppHelpers;
using ChinaCatSunflower.Repositories;
using ChinaCatSunflower.Services;

var builder = WebApplication.CreateSlimBuilder(args);

// Add services to the container.
var connection_string = builder.Configuration.GetConnectionString(ApplicationSettings.SQL_CONNECTION_NAME) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseNpgsql(connection_string
        , x => x.MigrationsHistoryTable("__ef_migrations_history", "public"))
        .ReplaceService<IHistoryRepository, MyHistoryRepository>();
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddCookie(o => {
    o.Cookie.IsEssential = true;
    o.Cookie.Name = ".chinacat.a";
    o.SlidingExpiration = true;
    o.Cookie.MaxAge = o.ExpireTimeSpan;
    o.Events.OnSigningOut = ctx => {
        ctx.HttpContext.Session.Clear();
        return Task.CompletedTask;
    };
});
builder.Services.AddRouting(o => {
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
});
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddAntiforgery(af => {
    af.Cookie.Name = ".chinacat.af";
});
builder.Services.AddHttpClient(ApplicationSettings.HTTP_CLIENT_OPEN_LIBRARY, client => {
    client.BaseAddress = new Uri("https://openlibrary.org/");
});
builder.Services.AddTransient<BookRetrievalService>();
builder.Services.AddTransient<FibLogRepository>();
builder.Services.AddTransient<BookRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope()) {
    await CreateRoles(scope.ServiceProvider);
}



app.Run();


async Task CreateRoles(IServiceProvider serviceProvider) {
    //initializing custom roles 
    var role_manager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var user_manager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string[] role_names = { Roles.Admin, Roles.User, Roles.SuperUser };

    foreach (var roleName in role_names) {
        var role_exist = await role_manager.RoleExistsAsync(roleName);
        // ensure that the role does not exist
        if (!role_exist) {
            //create the roles and seed them to the database: 
            await role_manager.CreateAsync(new IdentityRole(roleName));
        }
    }

    //Here you create the super admin who will maintain the web app
    await AddAdminUser(user_manager, "jason@dabsoftware.com", "password");
    
}

async Task AddAdminUser(UserManager<IdentityUser> user_manager, string username, string password) {
    var user = await user_manager.FindByEmailAsync(username);

    // check if the user exists
    if (user == null) {
        //Here you could create the super admin who will maintain the web app
        var seeduser = new IdentityUser {
            UserName = username,
            Email = username,
            EmailConfirmed = true
        };
        string seeduser_password = password;
        var create_power_user = await user_manager.CreateAsync(seeduser, seeduser_password);
        if (create_power_user.Succeeded) {
            //here we tie the new user to the role
            await user_manager.AddToRoleAsync(seeduser, Roles.Admin);
        }
    }
}

public static class Roles
{
    public const string Admin = "ADMIN";
    public const string SuperUser = "SUPERUSER";
    public const string User = "USER";
}