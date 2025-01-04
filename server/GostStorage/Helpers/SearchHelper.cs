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
}