using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class DocStatisticsRepository(DataContext context) : IDocStatisticsRepository
{
    public async Task<List<DocStatisticEntity>> GetAllAsync()
    {
        return await context.DocStatistics.ToListAsync();
    }

    public async Task AddAsync(DocStatisticEntity statistic)
    {
        await context.DocStatistics.AddAsync(statistic);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long docId)
    {
        var statistic = await context.DocStatistics.FirstOrDefaultAsync(stat => stat.DocId == docId);

        if (statistic is not null) context.DocStatistics.Remove(statistic);

        await context.SaveChangesAsync();
    }
}