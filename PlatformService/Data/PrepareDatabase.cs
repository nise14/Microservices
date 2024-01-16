using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepareDatabase
{
    public static async Task PreparePopulation(IApplicationBuilder app, bool isProduction)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            await SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction); ;
        }
    }

    private static async Task SeedData(AppDbContext? context, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("---> Attempting to apply migrations... ");
            try
            {
                context?.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not apply migrations {ex.Message}");
            }
        }

        if (!context!.platforms.Any())
        {
            Console.WriteLine("---> Seeding Data...");
            context!.platforms.AddRange(
                new Platform()
                {
                    Name = "Dotnet",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform()
                {
                    Name = "SQL Server Express",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform()
                {
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free"
                }
            );

            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("---> We already have data");
        }
    }
}