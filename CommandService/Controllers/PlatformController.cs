using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public PlatformController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> TestInboundConnection()
        {
            Console.WriteLine("--> Test successful");

            return Ok("Test okay");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDTO>>> GetAllPlatforms()
        {
            Console.WriteLine("--> Getting all platforms");
            var platformObjs = await _commandRepository.GetAllPlatorms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformObjs));
        }
    }
}
