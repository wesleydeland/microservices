using System;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateCommand(int platformId, Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        await _context.Commands.AddAsync(command);
    }

    public async Task CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        await _context.Platforms.AddAsync(platform);
    }

    public async Task<IEnumerable<Platform>> GetAllPlatorms()
    {
        return await _context.Platforms.AsNoTracking().ToListAsync();
    }

    public async Task<Command> GetCommand(int platformId, int commandId)
    {
        return await _context.Commands.AsNoTracking().FirstOrDefaultAsync(c => c.Id == commandId && c.PlatformId == platformId);
    }

    public async Task<IEnumerable<Command>> GetCommandsForPlatform(int platformId)
    {
        return await _context.Commands.AsNoTracking().Where(c => c.PlatformId == platformId).ToListAsync();
    }

    public async Task<bool> PlatformExists(int platformId)
    {
        return await _context.Platforms.AsNoTracking().AnyAsync(p => p.Id == platformId);
    }

    public async Task<bool> SaveChanges()
    {
        return (await _context.SaveChangesAsync()) >= 0;
    }
}
