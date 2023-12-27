using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class DocStatisticsService(IDocsRepository docsRepository, IDocStatisticsRepository docStatisticsRepository, IDocsService docsService)
    : IDocStatisticsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IDocStatisticsRepository _docStatisticsRepository = docStatisticsRepository;
    private readonly IDocsService _docsService = docsService;
    
    public async Task AddNewDocStatsAsync(long docId)
    {
        var statistic = new DocStatisticEntity { DocId = docId, Created = DateTime.UtcNow, Changed = DateTime.UtcNow };
        await _docStatisticsRepository.AddAsync(statistic);
    }

    public async Task UpdateViewsAsync(long docId)
    {
        await _docStatisticsRepository.UpdateViewsAsync(docId);
    }

    public async Task UpdateChangedAsync(long docId)
    {
        await _docStatisticsRepository.UpdateDateTimeAsync(docId);
    }

    public async Task DeleteAsync(long docId)
    {
        await _docStatisticsRepository.DeleteAsync(docId);
    }

    public async Task<ActionResult<List<DocWithViewsModel>>> GetViews(GetViewsModel model)
    {
        var docs = await _docsService.GetAllDocuments();
        var statistics = await _docStatisticsRepository.GetAllAsync();

        return new OkObjectResult(docs
            .Where(doc =>
            {
                var actualField = doc.Actual;
                var primaryField = doc.Primary;
                var statistic = statistics.FirstOrDefault(stat => stat.DocId == doc.DocId);

                return IsDocPassedFilter(actualField, primaryField, model, statistic);
            })
            .Select<GetDocumentResponseModel, DocWithViewsModel>(doc =>
            {
                var actualField = doc.Actual;
                var primaryField = doc.Primary;
                var statistic = statistics.FirstOrDefault(stat => stat.DocId == doc.DocId);

                return new DocWithViewsModel
                {
                    Designation = actualField.Designation ?? primaryField.Designation,
                    DocId = doc.DocId,
                    FullName = actualField.FullName ?? primaryField.FullName,
                    Views = statistic.Views
                };
            })
            .ToList());
    }

    private bool IsDocPassedFilter(FieldEntity actualField, FieldEntity primaryField,
        GetViewsModel model, DocStatisticEntity statistic)
    {
        return (actualField.Designation ?? primaryField.Designation).Contains(model.Designation) &&
               (actualField.ActivityField ?? primaryField.ActivityField).Contains(model.ActivityField) &&
               (actualField.CodeOKS ?? primaryField.CodeOKS).Contains(model.CodeOKS) &&
               (actualField.FullName ?? primaryField.FullName).Contains(model.FullName) &&
               ((model.StartDate <= statistic.Created && statistic.Created <= model.EndDate) ||
                (model.StartDate <= statistic.Changed && statistic.Changed <= model.EndDate));
    }
}