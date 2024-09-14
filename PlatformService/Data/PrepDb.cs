using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static async Task PrepPopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!, isProduction);
        }
    }

    private static async Task SeedData(AppDbContext context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Running Migrations...");
            try
            {
                context.Database.Migrate();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding Data...");

            await context.Platforms.AddRangeAsync(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Sql Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("--> We already got data");
        }
    }
}
