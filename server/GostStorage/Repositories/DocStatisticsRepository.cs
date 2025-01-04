using GostStorage.Data;
using GostStorage.Entities;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories;

public class DocStatisticsRepository(DataContext context) : IDocStatisticsRepository
{
    public async Task<List<UserAction>> GetAllAsync()
    {
        return await context.UserActions.ToListAsync();
    }

    public async Task AddAsync(UserAction statistic)
    {
        await context.UserActions.AddAsync(statistic);

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long docId)
    {
        var statistic = await context.UserActions.FirstOrDefaultAsync(stat => stat.DocId == docId);

        if (statistic is not null) context.UserActions.Remove(statistic);

        await context.SaveChangesAsync();
    }
}