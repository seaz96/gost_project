using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Statistic;
using GostStorage.Navigations;
using GostStorage.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Repositories.Concrete;

public class UserActionsRepository(DataContext context) : IUserActionsRepository
{
    public async Task AddAsync(UserAction statistic)
    {
        await context.UserActions.AddAsync(statistic);

        await context.SaveChangesAsync();
    }

    public async Task<List<DocumentViewsResponse>> GetViewsAsync(GetViewsModel model)
    {
        var actions = SearchHelper.GetFullDocumentQueryable(context)
            .Join(context.UserActions,
                d => d.DocId,
                a => a.DocId,
                (d, a) => new
                {
                    Document = d, Action = a
                })
            .Where(group => (string.IsNullOrEmpty(model.OrgBranch) || group.Action.OrgBranch == model.OrgBranch)
                            && (model.Designation == null || group.Document.Actual.Designation.Contains(
                                model.Designation)) &&
                            (model.ActivityField == null ||
                              (group.Document.Actual.ActivityField ?? group.Document.Primary.ActivityField ?? "")
                              .Contains(model.ActivityField)) &&
                            (model.CodeOks == null ||
                              (group.Document.Actual.CodeOks ?? group.Document.Primary.CodeOks ?? "").Contains(
                                  model.CodeOks)) &&
                            (model.StartYear == null || model.StartYear <= group.Action.Date) &&
                            (model.EndYear == null || group.Action.Date <= model.EndYear) &&
                            group.Action.Type == ActionType.View).ToListAsync();

        return (await actions)
            .GroupBy(stat => stat.Document)
            .Select(group => new DocumentViewsResponse()
            {
                DocId = group.Key.DocId,
                Designation = group.Key.Actual.Designation,
                Views = group.Count(),
                FullName = group.Key.Actual.FullName ?? group.Key.Primary.FullName,
            })
            .OrderByDescending(x => x.Views)
            .ToList();
    }

    public Task<List<UserActionDocumentModel>> GetActionsAsync(DocumentCountRequest model)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
            .Join(context.UserActions,
                d => d.DocId,
                a => a.DocId,
                (d, a) => new
                {
                    Document = d, Action = a
                })
            .Where(group => (model.StartDate == null || model.StartDate <= group.Action.Date) &&
                            (model.EndDate == null || group.Action.Date <= model.EndDate)
                            && group.Action.Type != ActionType.View
                            && group.Document.Status == model.Status)
            .Select(x => new UserActionDocumentModel
            {
                DocumentId = x.Document.DocId,
                Designation = x.Document.Actual.Designation,
                FullName = x.Document.Actual.FullName ?? x.Document.Primary.FullName ?? "",
                Action = x.Action.Type,
                Date = x.Action.Date
            })
            .ToListAsync();
    }

    public async Task DeleteAsync(long docId)
    {
        var statistic = await context.UserActions.FirstOrDefaultAsync(stat => stat.DocId == docId);

        if (statistic is not null) context.UserActions.Remove(statistic);

        await context.SaveChangesAsync();
    }
}