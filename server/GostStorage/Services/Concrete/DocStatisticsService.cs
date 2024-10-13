using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Stats;
using GostStorage.Navigations;
using GostStorage.Repositories.Interfaces;
using GostStorage.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Concrete;

public class DocStatisticsService(IDocsRepository docsRepository, IDocStatisticsRepository docStatisticsRepository, IDocsService docsService)
    : IDocStatisticsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;

    public async Task AddAsync(DocStatisticEntity statistic)
    {
        await docStatisticsRepository.AddAsync(statistic);
    }

    public async Task DeleteAsync(long docId)
    {
        await docStatisticsRepository.DeleteAsync(docId);
    }

    public async Task<IActionResult> GetViews(GetViewsModel model)
    {
        if (model.Designation is not null)
        {
            model.Designation = TextFormattingHelper.FormatDesignation(model.Designation);
        }

        var docs = await docsService.GetDocumentsAsync(new SearchParametersModel(), null, 10000, 0);
        var statistics = await docStatisticsRepository.GetAllAsync();

        return new OkObjectResult(statistics.Where(stat =>      
        {
            var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId && (string.IsNullOrEmpty(model.OrgBranch) || stat.OrgBranch == model.OrgBranch));

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
            if (doc is null)
                return null;
            return new DocWithViewsModel {DocId = doc.DocId, Designation = doc.Actual.Designation ?? doc.Primary.Designation,
                Views = stat.Views, FullName = doc.Actual.FullName ?? doc.Primary.FullName};
        })
        .Where(x => x is not null)
        .ToList());
    }

    public async Task<IActionResult> GetCount(GetCountOfDocsModel model)
    {
        var docs = await docsService.GetDocumentsAsync(new SearchParametersModel(), null, 10000, 0);
        var statistics = await docStatisticsRepository.GetAllAsync();

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
    
    private static bool IsGetViewsDocPassedFilter(FieldEntity actualField, FieldEntity primaryField,
        GetViewsModel model, DocStatisticEntity statistic)
    {
        return (model.Designation is not null ? (actualField.Designation ?? primaryField.Designation).Contains(model.Designation) : true) &&
               (model.ActivityField is not null ? (actualField.ActivityField ?? primaryField.ActivityField).Contains(model.ActivityField) : true) && 
               (model.CodeOKS is not null ? (actualField.CodeOKS ?? primaryField.CodeOKS).Contains(model.CodeOKS) : true) &&
               (model.StartYear is not null ? model.StartYear <= statistic.Date : true) &&
               (model.EndYear is not null ? statistic.Date <= model.EndYear : true) && 
               statistic.Action == ActionType.View;
    }
}