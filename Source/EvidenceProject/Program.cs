using Microsoft.EntityFrameworkCore;

namespace EvidenceProject;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Session
        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = "zajimavasusenka";
            options.IdleTimeout = TimeSpan.FromDays(15);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromDays(8);
        });
        // Stavitel
        builder.Services.AddDbContext<ProjectContext>(opt => {

        if (builder.Configuration.GetValue<bool>("UsePostgres"))
            opt.UseNpgsql(builder.Configuration["DatabaseConnection"]);
        else
            opt.UseSqlServer(builder.Configuration["DatabaseConnection"]);
        });

        builder.Services.AddControllersWithViews();
        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.Logger.LogInformation("Logging enabled!");

        app.Logger.LogInformation("Enabling https redirection.");
        app.UseHttpsRedirection();

        app.Logger.LogInformation("Enabling static file use.");
        app.UseStaticFiles();

        app.Logger.LogInformation("Enabling routing.");
        app.UseRouting();
        
        app.Logger.LogInformation("Enabling authorization.");
        app.UseAuthorization();
        
        app.Logger.LogInformation("Enabling sessions.");
        app.UseSession();
        
        app.Logger.LogInformation("Mapping routes.");
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Logger.LogInformation("Starting app.");
        app.Run();
    }
}
    