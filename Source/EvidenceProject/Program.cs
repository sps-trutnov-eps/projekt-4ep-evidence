using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
        var provider = builder.Configuration.GetValue("Provider", "SqlServer");
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
        builder.Services.AddDbContext<ProjectContext>(opt => _ = provider switch
        {
            "Postgres" => opt.UseNpgsql(
                builder.Configuration["DatabaseConnection"],
                x => x.MigrationsAssembly("Migrations.Postgres")),

            "SqlServer" => opt.UseSqlServer(
                builder.Configuration["DatabaseConnection"],
                x => x.MigrationsAssembly("Migrations.SqlServer")),

            _ => throw new Exception($"Unsupported provider: {provider}")
        });

        builder.Services.AddControllersWithViews();
        var app = builder.Build();

        var message = "\n   _________ __                 __   \n /   _____//  |______ ________/  |_ \n \\_____  \\\\   __\\__  \\\\_  __ \\   __\\\n /        \\|  |  / __ \\|  | \\/|  |  \n/_______  /|__| (____  /__|   |__|  \n        \\/           \\/             \n";


        app.Logger.LogInformation(message);

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
    