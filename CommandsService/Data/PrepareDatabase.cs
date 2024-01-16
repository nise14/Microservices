using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data;

public static class PrepareDatabase
{
    public static async Task PreparePopulation(IApplicationBuilder app)
    {
        await using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            var platforms = grpcClient!.ReturnAllPlatforms();

            await SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>()!, platforms!);
        }
    }

    private static async Task SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
    {
        Console.WriteLine("Seeding new platforms...");

        foreach (var platform in platforms!)
        {
            if (!await repository.ExternalPlatformExistsAsync(platform.ExternalId))
            {
                await repository.CreatePlatformAsync(platform);
            }

            await repository.SaveChangesAsync();
        }
    }
}