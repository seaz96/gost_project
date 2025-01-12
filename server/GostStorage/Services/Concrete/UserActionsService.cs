using GostStorage.Entities;
using GostStorage.Helpers;
using GostStorage.Models.Statistic;
using GostStorage.Repositories.Abstract;
using GostStorage.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Concrete;

public class UserActionsService(IUserActionsRepository userActionsRepository) : IUserActionsService
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

    public async Task<IActionResult> GetActionsAsync(DocumentCountRequest model)
    {
        return new OkObjectResult(await userActionsRepository.GetActionsAsync(model));
    }
}