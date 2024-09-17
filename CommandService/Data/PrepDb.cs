using System;
using CommandService.Models;
using CommandService.SyncDataServices.Grpc;

namespace CommandService.Data;

public static class PrepDb
{
    public static async Task PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = await grpcClient!.ReturnAllPlatforms();

            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>()!, platforms);
        }
    }

    private static async void SeedData(ICommandRepository repo, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("--> Seeding new platforms...");

        foreach (var platform in platforms)
        {
            if (!await repo.ExternalPlatformExists(platform.ExternalId))
            {
                await repo.CreatePlatform(platform);
            }
        }

        await repo.SaveChanges();
    }
}
