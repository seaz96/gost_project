namespace Gost_Project.Services.Abstract;

public interface IDocStatisticsService
{
    public Task AddNewDocStatsAsync(long docId);

    public Task UpdateViewsAsync(long docId);

    public Task UpdateChangedAsync(long docId);

    public Task DeleteAsync(long docId);
}