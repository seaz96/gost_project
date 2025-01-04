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

public class UserActionsService(
        IUserActionsRepository userActionsRepository,
        IDocumentsService documentsService)
    : IUserActionsService
{
    public async Task AddAsync(UserAction statistic)
    {
        await userActionsRepository.AddAsync(statistic);
    }

    public async Task DeleteAsync(long docId)
    {
        await userActionsRepository.DeleteAsync(docId);
    }

    public async Task<IActionResult> GetViews(GetViewsModel model)
    {
        if (model.Designation is not null)
        {
            model.Designation = TextFormattingHelper.FormatDesignation(model.Designation);
        }

        return new OkObjectResult(await userActionsRepository.GetViewsAsync(model));
    }

    public async Task<IActionResult> GetCount(DocumentCountRequest model)
    {
        var docs = await documentsService.GetDocumentsAsync(new GetDocumentRequest
        {
            Status = model.Status
        });
        var statistics = await userActionsRepository.GetAllAsync();

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
}