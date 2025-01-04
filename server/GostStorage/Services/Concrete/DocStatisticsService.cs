using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Docs;
using GostStorage.Models.Statistic;
using GostStorage.Navigations;
using GostStorage.Repositories;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GostStorage.Services.Concrete;

//note(azanov.n): этот класс выглядит очень страшно, переделать бы
public class DocStatisticsService(
        IDocStatisticsRepository docStatisticsRepository,
        IDocumentsService documentsService)
    : IDocStatisticsService
{
    public async Task AddAsync(UserAction statistic)
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

        var docs = await documentsService.GetDocumentsAsync(new GetDocumentRequest
        {
            Name = model.Designation,
            ActivityField = model.ActivityField,
            CodeOks = model.CodeOks
        });
        var statistics = await docStatisticsRepository.GetAllAsync();

        return new OkObjectResult(statistics.Where(stat =>
            {
                var doc = docs.FirstOrDefault(x =>
                    x.DocId == stat.DocId &&
                    (string.IsNullOrEmpty(model.OrgBranch) || stat.OrgBranch == model.OrgBranch));

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

                try
                {
                    var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId);

                    return new DocumentViewsRequest
                    {
                        DocId = doc.DocId, Designation = doc.Actual.Designation ?? doc.Primary.Designation,
                        Views = stat.Views, FullName = doc.Actual.FullName ?? doc.Primary.FullName
                    };
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e.Message);
                    return null;
                }
            })
            .Where(x => x is not null)
            .ToList());
    }

    public async Task<IActionResult> GetCount(DocumentCountRequest model)
    {
        var docs = await documentsService.GetDocumentsAsync(new GetDocumentRequest
        {
            Status = model.Status
        });
        var statistics = await docStatisticsRepository.GetAllAsync();

        var filteredStats = statistics
            .Where(stat => (model.StartDate is not null ? model.StartDate <= stat.Date : true) &&
                           (model.EndDate is not null ? stat.Date <= model.EndDate : true)
                           && stat.Action != ActionType.View)
            .Where(stat =>
            {
                try
                {
                    var doc = docs.FirstOrDefault(x => x.DocId == stat.DocId);
                    return doc != null && doc.Status == model.Status;
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e.Message);
                    return false;
                }
            });

        var count = filteredStats.GroupBy(stat => stat.DocId).Count();

        return new OkObjectResult(new
        {
            Count = count,
            Stats = filteredStats.Select(x =>
            {
                try
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
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e.Message);
                    return null;
                }
            })
        });
    }

    private static bool IsGetViewsDocPassedFilter(Field actualField, Field primaryField,
        GetViewsModel model, UserAction statistic)
    {
        return (model.Designation is not null
                   ? (actualField.Designation ?? primaryField.Designation).Contains(model.Designation)
                   : true) &&
               (model.ActivityField is not null
                   ? (actualField.ActivityField ?? primaryField.ActivityField).Contains(model.ActivityField)
                   : true) &&
               (model.CodeOks is not null
                   ? (actualField.CodeOks ?? primaryField.CodeOks).Contains(model.CodeOks)
                   : true) &&
               (model.StartYear is not null ? model.StartYear <= statistic.Date : true) &&
               (model.EndYear is not null ? statistic.Date <= model.EndYear : true) &&
               statistic.Action == ActionType.View;
    }
}