using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class DocStatisticsService(IDocsRepository docsRepository, IDocStatisticsRepository docStatisticsRepository, IDocsService docsService)
    : IDocStatisticsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IDocStatisticsRepository _docStatisticsRepository = docStatisticsRepository;
    private readonly IDocsService _docsService = docsService;
    
    public async Task AddAsync(DocStatisticEntity statistic)
    {
        await _docStatisticsRepository.AddAsync(statistic);
    }

    public async Task DeleteAsync(long docId)
    {
        await _docStatisticsRepository.DeleteAsync(docId);
    }

    public async Task<IActionResult> GetViews(GetViewsModel model)
    {
        var docs = await _docsService.GetAllDocuments();
        var statistics = await _docStatisticsRepository.GetAllAsync();

        return new OkObjectResult(statistics.Where(stat =>      
            {
                var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId);
                return doc is not null && IsGetViewsDocPassedFilter(doc.Actual, doc.Primary, model, stat);
            })
            .GroupBy(stat => stat.DocId)
            .Select(group => new
            {
                DocId = group.Key,
                Views = group.Count()
            })
            .OrderByDescending(stat => stat.Views)
            .Select(stat =>
            {
                var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId);
                return new DocWithViewsModel {DocId = doc.DocId, Designation = doc.Actual.Designation ?? doc.Primary.Designation,
                    Views = stat.Views, FullName = doc.Actual.FullName ?? doc.Primary.FullName};
            })
            .ToList());
    }

    public async Task<IActionResult> GetCount(GetCountOfDocsModel model)
    {
        var docs = await _docsService.GetAllDocuments();
        var statistics = await _docStatisticsRepository.GetAllAsync();

        var filteredStats = statistics
            .Where(stat => (model.StartDate is not null ? model.StartDate <= stat.Date : true) &&
                           (model.EndDate is not null ? stat.Date <= model.EndDate : true) 
                           && stat.Action != ActionType.View)
            .Where(stat =>
            {
                var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId);
                return doc != null && (model.Status is not null ? doc.Primary.Status == model.Status : true);
            });

        var count = filteredStats.GroupBy(stat => stat.DocId).Count();
        
        return new OkObjectResult(new { Count = count,
            Stats = filteredStats.Select(x =>
            {
                var doc = docs.FirstOrDefault(doc => doc.DocId == x.DocId);
                return new
                {
                    DocId = x.DocId,
                    Designation = doc.Actual.Designation ?? doc.Primary.Designation,
                    FullName = doc.Actual.FullName ?? doc.Primary.FullName,
                    Action = x.Action.ToString(),
                    Date = x.Date
                };
            })});
        
    }
    
    private bool IsGetViewsDocPassedFilter(FieldEntity actualField, FieldEntity primaryField,
        GetViewsModel model, DocStatisticEntity statistic)
    {
        return (model.Designation is not null ? (actualField.Designation ?? primaryField.Designation).Contains(model.Designation) : true) &&
               (model.ActivityField is not null ? (actualField.ActivityField ?? primaryField.ActivityField).Contains(model.ActivityField) : true) && 
               (model.CodeOKS is not null ? (actualField.CodeOKS ?? primaryField.CodeOKS).Contains(model.CodeOKS) : true) &&
               (model.StartDate is not null ? model.StartDate <= statistic.Date : true) &&
               (model.EndDate is not null ? statistic.Date <= model.EndDate : true) && 
               statistic.Action == ActionType.View;
    }
}