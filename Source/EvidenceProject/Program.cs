using EvidenceProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace EvidenceProject;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        // TODO zkusit najit admina
        // TODO pokud nebude ulozit do DB

        // konfigurace z appsettings.json
        IConfiguration configurationBUilder = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", false).Build();
        configurationBUilder.GetValue<string>("Admin:username");
        configurationBUilder.GetValue<string>("Admin:password");


        // Session
        builder.Services.AddSession(options =>
        {
            options.Cookie.Name = "zajimavasusenka";
            options.IdleTimeout = TimeSpan.FromDays(15);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.MaxAge = TimeSpan.FromDays(8);
        });

        builder.Services.AddDbContext<ProjectContext>(opt =>
            opt.UseSqlServer(
                builder.Configuration["DatabaseConnection"]));

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    