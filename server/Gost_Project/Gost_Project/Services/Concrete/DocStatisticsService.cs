using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;

namespace Gost_Project.Services.Concrete;

public class DocStatisticsService(IDocsRepository docsRepository, IDocStatisticsRepository docStatisticsRepository)
    : IDocStatisticsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IDocStatisticsRepository _docStatisticsRepository = docStatisticsRepository;
    
    public async Task AddNewDocStatsAsync(long docId)
    {
        var statistic = new DocStatisticEntity { DocId = docId, Created = DateTime.UtcNow, Changed = DateTime.UtcNow };
        await _docStatisticsRepository.AddAsync(statistic);
    }

    public async Task UpdateViewsAsync(long docId)
    {
        await _docStatisticsRepository.UpdateViewsAsync(docId);
    }

    public async Task UpdateChangedAsync(long docId)
    {
        await _docStatisticsRepository.UpdateDateTimeAsync(docId);
    }

    public async Task DeleteAsync(long docId)
    {
        await _docStatisticsRepository.DeleteAsync(docId);
    }
}