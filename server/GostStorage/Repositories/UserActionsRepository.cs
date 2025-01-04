using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Statistic;
using GostStorage.Navigations;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GostStorage.Repositories;

public class UserActionsRepository(DataContext context) : IUserActionsRepository
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

    public Task<List<DocumentViewsResponse>> GetViewsAsync(GetViewsModel model)
    {
        return SearchHelper.GetFullDocumentQueryable(context)
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
                            (model.ActivityField == null || (group.Document.Actual.ActivityField ?? group.Document.Primary.ActivityField)
                                .Contains(model.ActivityField)) &&
                            (model.CodeOks == null || (group.Document.Actual.CodeOks ?? group.Document.Primary.CodeOks).Contains(
                                model.CodeOks)) &&
                            (model.StartYear == null || model.StartYear <= group.Action.Date) &&
                            (model.EndYear == null || group.Action.Date <= model.EndYear) &&
                            group.Action.Action == ActionType.View)
            .GroupBy(stat => stat.Document)
            .Select(group => new
            {
                Document = group.Key,
                Views = group.Count()
            })
            .Select(stat => new DocumentViewsResponse
                {
                    DocId = stat.Document.DocId,
                    Designation = stat.Document.Actual.Designation,
                    Views = stat.Views,
                    FullName = stat.Document.Actual.FullName ?? stat.Document.Primary.FullName
                })
            .OrderByDescending(group => group.Views)
            .ToListAsync();
    }

    public async Task DeleteAsync(long docId)
    {
        var statistic = await context.UserActions.FirstOrDefaultAsync(stat => stat.DocId == docId);

        if (statistic is not null) context.UserActions.Remove(statistic);

        await context.SaveChangesAsync();
    }
}