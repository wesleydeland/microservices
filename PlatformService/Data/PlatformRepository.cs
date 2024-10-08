using System;
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly AppDbContext _context;
    public PlatformRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        await _context.Platforms.AddAsync(platform);
    }

    public async Task<IEnumerable<Platform>> GetAllPlatforms()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<Platform> GetPlatformById(int id)
    {
        return await _context.Platforms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}
