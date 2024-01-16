using System.ComponentModel.Design;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/command/platform/{platformId}/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommandReadDto>>> GetCommandsForPlatform(int platformId)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

        if (!await _repository.PlatformExistsAsync(platformId))
        {
            return NotFound();
        }

        var commands = await _repository.GetCommandsForPlatformAsync(platformId);

        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public async Task<ActionResult<CommandReadDto>> GetCommandForPlatform(int platformId, int commandId)
    {
        Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

        if (!await _repository.PlatformExistsAsync(platformId))
        {
            return NotFound();
        }

        var command = await _repository.GetCommandAsync(platformId, commandId);

        if (command is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandReadDto>(command));
    }

    [HttpPost]
    public async Task<ActionResult<CommandReadDto>> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

        if (!await _repository.PlatformExistsAsync(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(commandCreateDto);

        await _repository.CreateCommandAsync(platformId, command);
        await _repository.SaveChangesAsync();

        var CommandReadDto = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform),
            new
            {
                platformId,
                CommandId = CommandReadDto.Id
            });
    }
}