using GostStorage.Data;
using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using Microsoft.EntityFrameworkCore;

namespace GostStorage.Helpers;

public static class SearchHelper
{
    public static async Task<List<long>> SearchFields(GetDocumentRequest parameters, DataContext _context)
    {
        var primary = await _context.PrimaryFields
            .Where(f => parameters.CodeOks == null || (f.CodeOks ?? "").ToLower()
                .Contains(parameters.CodeOks.ToLower()))
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
            .AsSingleQuery()
            .Select(f => f.Id)
            .ToListAsync();
        
        primary.AddRange(await _context.ActualFields
            .Where(f => parameters.CodeOks == null || (f.CodeOks ?? "").ToLower()
                .Contains(parameters.CodeOks.ToLower()))
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
            .AsSingleQuery()
            .Select(f => f.Id)
            .ToListAsync());

        return primary;
    }

    public static IQueryable<FullDocument> ApplyFilters(
        this IQueryable<FullDocument> queryable,
        GetDocumentRequest? parameters)
    {
        if (parameters is null) return queryable;
        
        return queryable
            .Where(f => parameters.CodeOks == null
                        || (f.Primary.CodeOks ?? "").ToLower().Contains(parameters.CodeOks.ToLower())
                        || (f.Actual.CodeOks ?? "").ToLower().Contains(parameters.CodeOks.ToLower()))
            .Where(f => parameters.ActivityField == null
                        || (f.Primary.ActivityField ?? "").ToLower().Contains(parameters.ActivityField.ToLower())
                        || (f.Actual.ActivityField ?? "").ToLower().Contains(parameters.ActivityField.ToLower()))
            .Where(f => parameters.AdoptionLevel == null
                        || f.Primary.AdoptionLevel != parameters.AdoptionLevel
                        || f.Actual.AdoptionLevel != parameters.AdoptionLevel)
            .Where(f => parameters.Name == null ||
                        (f.Primary.Designation ?? "").ToLower().Contains(parameters.Name.ToLower()) ||
                        (f.Primary.FullName ?? "").ToLower().Contains(parameters.Name.ToLower()) ||
                        (f.Actual.Designation ?? "").ToLower().Contains(parameters.Name.ToLower()) ||
                        (f.Actual.FullName ?? "").ToLower().Contains(parameters.Name.ToLower()))
            .Where(f => parameters.AcceptanceYear == null
                        || f.Primary.AcceptanceYear.Value == parameters.AcceptanceYear.Value
                        || f.Actual.AcceptanceYear.Value == parameters.AcceptanceYear.Value)
            .Where(f => parameters.CommissionYear == null
                        || f.Primary.CommissionYear.Value == parameters.CommissionYear.Value
                        || f.Actual.CommissionYear.Value == parameters.CommissionYear.Value)
            .Where(f => parameters.Author == null
                        || (f.Primary.Author ?? "").ToLower().Contains(parameters.Author.ToLower())
                        || (f.Actual.Author ?? "").ToLower().Contains(parameters.Author.ToLower()))
            .Where(f => parameters.AcceptedFirstTimeOrReplaced == null
                        || (f.Primary.AcceptedFirstTimeOrReplaced ?? "").ToLower().Contains(parameters.AcceptedFirstTimeOrReplaced.ToLower())
                        || (f.Actual.AcceptedFirstTimeOrReplaced ?? "").ToLower().Contains(parameters.AcceptedFirstTimeOrReplaced.ToLower()))
            .Where(f => parameters.KeyWords == null
                        || (f.Primary.KeyWords ?? "").ToLower().Contains(parameters.KeyWords.ToLower())
                        || (f.Actual.KeyWords ?? "").ToLower().Contains(parameters.KeyWords.ToLower()))
            .Where(f => parameters.ApplicationArea == null
                        || (f.Primary.ApplicationArea ?? "").ToLower().Contains(parameters.ApplicationArea.ToLower())
                        || (f.Actual.ApplicationArea ?? "").ToLower().Contains(parameters.ApplicationArea.ToLower()))
            .Where(f => parameters.DocumentText == null
                        || (f.Primary.DocumentText ?? "").ToLower().Contains(parameters.DocumentText.ToLower())
                        || (f.Actual.DocumentText ?? "").ToLower().Contains(parameters.DocumentText.ToLower()))
            .Where(f => parameters.Changes == null
                        || (f.Primary.Changes ?? "").ToLower().Contains(parameters.Changes.ToLower())
                        || (f.Actual.Changes ?? "").ToLower().Contains(parameters.Changes.ToLower()))
            .Where(f => parameters.Amendments == null
                        || (f.Primary.Amendments ?? "").ToLower().Contains(parameters.Amendments.ToLower())
                        || (f.Actual.Amendments ?? "").ToLower().Contains(parameters.Amendments.ToLower()))
            .Where(f => parameters.Content == null
                        || (f.Primary.Content ?? "").ToLower().Contains(parameters.Content.ToLower())
                        || (f.Actual.Content ?? "").ToLower().Contains(parameters.Content.ToLower()))
            .Where(f => parameters.Harmonization == null
                        || f.Primary.Harmonization == parameters.Harmonization
                        || f.Actual.Harmonization == parameters.Harmonization);
    }

    public static SearchDocument SplitFieldsToIndexDocument(long documentId, Field primary, Field actual)
    {
        return new SearchDocument
        {
            Id = documentId,
            Designation = actual.Designation ?? primary.Designation,
            FullName = actual.FullName ?? primary.FullName,
            CodeOks = actual.CodeOks ?? primary.CodeOks,
            ActivityField = actual.ActivityField ?? primary.ActivityField,
            AcceptanceYear = actual.AcceptanceYear ?? primary.AcceptanceYear,
            CommissionYear = actual.CommissionYear ?? primary.CommissionYear,
            Author = actual.Author ?? primary.Author,
            AcceptedFirstTimeOrReplaced = actual.AcceptedFirstTimeOrReplaced ?? primary.AcceptedFirstTimeOrReplaced,
            Content = actual.Content ?? primary.Content,
            KeyWords = actual.KeyWords ?? primary.KeyWords,
            ApplicationArea = actual.ApplicationArea ?? primary.ApplicationArea,
            AdoptionLevel = actual.AdoptionLevel ?? primary.AdoptionLevel,
            Changes = actual.Changes ?? primary.Changes,
            Amendments = actual.Amendments ?? primary.Amendments,
            Harmonization = primary.Harmonization,
        };
    }
    
    internal static IQueryable<FullDocument> GetFullDocumentQueryable(DataContext context)
    {
        return context.Documents
            .Join(context.PrimaryFields,
                d => d.Id,
                f => f.DocId,
                (d, p) => new { d, p })
            .Join(context.ActualFields,
                g => g.d.Id,
                f => f.DocId,
                (g, a) => new FullDocument
                {
                    DocId = g.d.Id,
                    Actual = a,
                    Primary = g.p,
                    Status = g.d.Status
                });
    }
}