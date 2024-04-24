using GostStorage.Domain.Models;
using GostStorage.Domain.Navigations;
using GostStorage.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Infrastructure.Helpers;

public static class SearchHelper
{
    public static async Task<List<long>> SearchFields(SearchParametersModel parameters, bool? isValid, DataContext _context)
    {
        return await _context.Fields
            .Where(f => parameters.CodeOKS == null || (f.CodeOKS ?? "").ToLower()
                .Contains(parameters.CodeOKS.ToLower()))
            .Where(f => parameters.ActivityField == null || (f.ActivityField ?? "").ToLower()
                .Contains(parameters.ActivityField.ToLower()))
            .Where(f => parameters.AdoptionLevel == null || f.AdoptionLevel != parameters.AdoptionLevel)
            .Where(f => parameters.Name == null || 
                        (f.Designation ?? "").ToLower()
                            .Contains(parameters.Name.ToLower()) || 
                        (f.FullName ?? "").ToLower()
                            .Contains(parameters.Name.ToLower()))
            .Where(f => parameters.AcceptanceYear == null || f.AcceptanceYear.Value == parameters.AcceptanceYear.Value)
            .Where(f => parameters.CommissionYear == null || f.CommissionYear.Value == parameters.CommissionYear.Value)
            .Where(f => parameters.Author == null || (f.Author ?? "").ToLower()
                .Contains(parameters.Author.ToLower()))
            .Where(f => parameters.AcceptedFirstTimeOrReplaced == null || (f.AcceptedFirstTimeOrReplaced ?? "").ToLower()
                .Contains(parameters.AcceptedFirstTimeOrReplaced.ToLower()))
            .Where(f => parameters.KeyWords == null || (f.KeyWords ?? "").ToLower()
                .Contains(parameters.KeyWords.ToLower()))
            .Where(f => parameters.ApplicationArea == null || (f.ApplicationArea ?? "").ToLower()
                .Contains(parameters.ApplicationArea.ToLower()))
            .Where(f => parameters.DocumentText == null || (f.DocumentText ?? "").ToLower()
                .Contains(parameters.DocumentText.ToLower()))
            .Where(f => parameters.Changes == null || (f.Changes ?? "").ToLower()
                .Contains(parameters.Changes.ToLower()))
            .Where(f => parameters.Amendments == null || (f.Amendments ?? "").ToLower()
                .Contains(parameters.Amendments.ToLower()))
            .Where(f => parameters.Content == null || (f.Content ?? "").ToLower()
                .Contains(parameters.Content.ToLower()))
            .Where(f => parameters.Harmonization == null || f.Harmonization == parameters.Harmonization)
            .Where(f => isValid == null || isValid.Value
                                                    ? f.Status == DocStatuses.Valid
                                                    : f.Status != DocStatuses.Valid && f.Status != DocStatuses.Inactive)
            .AsSingleQuery()
            .Select(f => f.Id)
            .ToListAsync();
    }
}