using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepository
{
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Platform>> GetAllPlatformsAsync();
    Task CreatePlatformAsync(Platform platform);
    Task<bool> PlatformExistsAsync(int platformId);
    Task<bool> ExternalPlatformExistsAsync(int externalPlatformId);
    Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);
    Task<Command?> GetCommandAsync(int platformId, int commandId);
    Task CreateCommandAsync(int platformId, Command command);
}