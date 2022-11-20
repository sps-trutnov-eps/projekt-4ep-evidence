using Microsoft.EntityFrameworkCore;

namespace EvidenceProject;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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
        builder.Services.AddDbContext<ProjectContext>(opt =>
            opt.UseSqlServer(
                builder.Configuration["DatabaseConnection"]));

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

        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
    