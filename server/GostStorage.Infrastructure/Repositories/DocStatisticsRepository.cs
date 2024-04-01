using GostStorage.Domain.Entities;
using GostStorage.Domain.Repositories;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Infrastructure.Repositories;

public class DocStatisticsRepository(DataContext context) : IDocStatisticsRepository
{
    private readonly DataContext _context = context;

    public async Task<List<DocStatisticEntity>> GetAllAsync()
    {
        return await _context.DocStatistics.ToListAsync();
    }

    public async Task AddAsync(DocStatisticEntity statistic)
    {
        await _context.DocStatistics.AddAsync(statistic);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(long docId)
    {
        var statistic = await _context.DocStatistics.FirstOrDefaultAsync(stat => stat.DocId == docId);

        if (statistic is not null)
        {
            _context.DocStatistics.Remove(statistic);
        }

        await _context.SaveChangesAsync();
    }
}