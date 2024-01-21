using Gost_Project.Data.Entities;
using Gost_Project.Data.Entities.Navigations;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Helpers;
using Gost_Project.Services.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Services.Concrete;

public class DocsService(IDocsRepository docsRepository, IFieldsRepository fieldsRepository, IReferencesRepository referencesRepository) : IDocsService
{
    private readonly IDocsRepository _docsRepository = docsRepository;
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    
    public async Task<long> AddNewDocAsync(FieldEntity primaryField)
    {
        var actualField = new FieldEntity();

        var primaryId = await _fieldsRepository.AddAsync(primaryField);
        var actualId = await _fieldsRepository.AddAsync(actualField);

        var doc = new DocEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };
        var docId = await _docsRepository.AddAsync(doc);

        return docId;
    }

    public async Task<IActionResult> DeleteDocAsync(long id)
    {
        var doc = await _docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        await _fieldsRepository.DeleteAsync(doc.PrimaryFieldId);
        await _fieldsRepository.DeleteAsync(doc.ActualFieldId);
        
        await _docsRepository.DeleteAsync(id);

        return new OkObjectResult("Document deleted successfully.");
    }

    public async Task<IActionResult> ChangeStatusAsync(long id, DocStatuses status)
    {
        var doc = await _docsRepository.GetByIdAsync(id);

        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        var primaryField = await _fieldsRepository.GetByIdAsync(doc.PrimaryFieldId);
        var actualField = await _fieldsRepository.GetByIdAsync(doc.ActualFieldId);
        
        if (primaryField is not null)
        {
            primaryField.Status = status;
            await _fieldsRepository.UpdateAsync(primaryField);
        }
        if (actualField is not null)
        {
            actualField.Status = status;
            await _fieldsRepository.UpdateAsync(actualField);
        }

        return new OkObjectResult("Status changed successfully.");
    }

    public async Task<ActionResult<GetDocumentResponseModel>> GetDocument(long id)
    {
        var doc = await _docsRepository.GetByIdAsync(id);
        
        if (doc is null)
        {
            return new UnprocessableEntityObjectResult($"Document with id {id} not found.");
        }
        
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();
        
        var references = (await _referencesRepository.GetAllAsync())
            .Where(reference => reference.ParentalDocId == id)
            .Select(reference =>
            {
                var docRef = docs.FirstOrDefault(x => x.Id == reference.ChildDocId);
                var primary = fields.Find(field => field.Id == docRef.PrimaryFieldId);
                var actual = fields.Find(field => field.Id == docRef.ActualFieldId);

                return new DocWithStatusModel
                {
                    DocId = docRef.Id,
                    Designation = actual.Designation ?? primary.Designation,
                    Status = primary.Status
                };
            })
            .ToList();
        
        var result = new GetDocumentResponseModel
        {
            Primary = await _fieldsRepository.GetByIdAsync(doc.PrimaryFieldId),
            Actual = await _fieldsRepository.GetByIdAsync(doc.ActualFieldId.Value),
            DocId = doc.Id,
            References = references
        };

        return new OkObjectResult(result);
    }

    public async Task<List<GetDocumentResponseModel>> GetAllDocuments(SearchParametersModel parameters, bool isValid)
    {
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();

        var docsWithFields = docs.Select(doc => new GetDocumentResponseModel
            { Primary = fields.Find(field => field.Id == doc.PrimaryFieldId),
                Actual = fields.Find(field => field.Id == doc.ActualFieldId),
                DocId = doc.Id })
            .Where(doc =>
            {
                if (isValid)
                    return doc.Primary.Status == DocStatuses.Valid;
                return doc.Primary.Status != DocStatuses.Valid;
            })
            .ToList();
        
        SearchHelper.GetMatchingDocs(docsWithFields, parameters);

        return docsWithFields;
    }
    
    public async Task<List<GetDocumentResponseModel>> GetAllDocuments(SearchParametersModel parameters)
    {
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();

        var docsWithFields = docs.Select(doc => new GetDocumentResponseModel
            { Primary = fields.Find(field => field.Id == doc.PrimaryFieldId),
                Actual = fields.Find(field => field.Id == doc.ActualFieldId),
                DocId = doc.Id })
            .ToList();
        
        return SearchHelper.GetMatchingDocs(docsWithFields, parameters);
    }
    
    public async Task<List<GetDocumentResponseModel>> GetAllDocuments()
    {
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();

        return docs.Select(doc => new GetDocumentResponseModel
            { Primary = fields.Find(field => field.Id == doc.PrimaryFieldId),
                Actual = fields.Find(field => field.Id == doc.ActualFieldId),
                DocId = doc.Id })
            .ToList();
    }

    public async Task<List<DesigntaionIdDocModel>> GetDesignationIdDocs()
    {
        var docs = await _docsRepository.GetAllAsync();
        var fields = await _fieldsRepository.GetAllAsync();

        return docs.Select(doc => new DesigntaionIdDocModel
            { Id = doc.Id, Designation = new Func<string>(() =>
                {
                    var primary = fields.Find(field => field.Id == doc.PrimaryFieldId);
                    var actual = fields.Find(field => field.Id == doc.ActualFieldId);

                    return (actual.Designation ?? primary.Designation);
                }
                )()})
            .ToList();
    }
}