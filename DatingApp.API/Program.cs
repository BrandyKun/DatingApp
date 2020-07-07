using System;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using ( var scope = host.Services.CreateScope())
            {
             var Services= scope.ServiceProvider;
             try
             {
                var contex = Services.GetRequiredService<DataContext>();
                var userManager = Services.GetRequiredService<UserManager<User>>();
                var roleManager = Services.GetRequiredService<RoleManager<Role>>();
                contex.Database.Migrate();
                Seed.SeedUsers(userManager, roleManager);
             }
             catch (Exception ex)
             {
                 var logger = Services.GetRequiredService<ILogger<Program>>();
                 logger.LogError(ex, "An Error occured during migration");
             }   
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
