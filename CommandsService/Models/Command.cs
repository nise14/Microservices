namespace CommandsService.Models;

public class Command
{
    public int Id { get; set; }
    public string HowTo { get; set; } = null!;
    public string CommandLine { get; set; } = null!;
    public int PlatformId { get; set; }
    public Platform Platform { get; set; } = new();
}