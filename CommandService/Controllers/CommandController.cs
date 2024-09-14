using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platform/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepository _repository;
        private readonly IMapper _mapper;

        public CommandController(ICommandRepository commandRepository, IMapper mapper)
        {
            _repository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandReadDTO>>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!await _repository.PlatformExists(platformId))
                return NotFound();

            var commands = await _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public async Task<ActionResult<CommandReadDTO>> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!await _repository.PlatformExists(platformId))
                return NotFound();

            var command = await _repository.GetCommand(platformId, commandId);
            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDTO>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandReadDTO>> CreateCommandForPlatform(int platformId, CommandCreateDTO commandCreateDTO)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!await _repository.PlatformExists(platformId))
                return NotFound();

            var command = _mapper.Map<Command>(commandCreateDTO);
            command.PlatformId = platformId;

            await _repository.CreateCommand(platformId, command);
            await _repository.SaveChanges();

            var commandReadDTO = _mapper.Map<CommandReadDTO>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDTO.Id }, commandReadDTO);
        }

    }
}
