using System;
using Microsoft.AspNetCore.Builder;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static async Task PrepPopulation(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
        }
    }

    private static async Task SeedData(AppDbContext context)
    {
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
