using GostStorage.Entities;
using GostStorage.Models.Docs;
using GostStorage.Models.Search;
using GostStorage.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Services.Abstract;

public interface IDocumentsService
{
    Task<long> AddDocumentAsync(PrimaryField primaryField, DocumentStatus status);

    Task<bool> DeleteDocumentAsync(long id);

    Task<IActionResult> ChangeStatusAsync(long id, DocumentStatus status);
    
    Task<FullDocument?> GetDocumentAsync(long id);

    Task<Document?> GetDocumentByDesignationAsync(string designation);

    Task<List<FullDocument>> GetDocumentsAsync(
        SearchQuery? parameters);

    Task<int> GetDocumentsCountAsync(SearchQuery? parameters);

    Task<bool> UploadFileForDocumentAsync(IFormFile file, long docId);

    Task<List<GeneralDocumentInfoModel>> SearchAsync(SearchQuery query);
    
    Task<int> SearchCountAsync(SearchQuery query);

    Task IndexAllDocumentsAsync();
}