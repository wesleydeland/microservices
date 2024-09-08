using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepository _repository;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepository platformRepository, IMapper mapper)
        {
            _repository = platformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDTO>>> GetPlatforms()
        {
            Console.WriteLine("--> Getting platforms");
            var platformModels = await _repository.GetAllPlatforms();

            return Ok(_mapper.Map<List<PlatformReadDTO>>(platformModels));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformReadDTO>> GetPlatformById(int id)
        {
            var platformModel = await _repository.GetPlatformById(id);

            if (platformModel is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platformModel));
            }

        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platformCreateDTO)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDTO);
            await _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var readDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { id = readDTO.Id }, readDTO);
        }
    }
}
