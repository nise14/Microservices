using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/command/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public PlatformController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> TestInBoundConnection()
    {
        Console.WriteLine("---> Inbound POST # Command Service");
        return Ok(await Task.FromResult("Inbound test of from platform Controller"));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetPlatforms()
    {
        Console.WriteLine("---> Getting Platforms from CommandService");

        var platformItems = await _repository.GetAllPlatformsAsync();

        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
}