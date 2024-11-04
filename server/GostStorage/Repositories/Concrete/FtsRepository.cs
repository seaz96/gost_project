using GostStorage.Models.Docs;
using GostStorage.Repositories.Interfaces;

namespace GostStorage.Repositories.Concrete;

public class FtsRepository : ISearchRepository
{
    public Task<FtsSearchEntity> SearchValidFieldsAsync(FtsSearchQuery query)
    {
        throw new NotImplementedException();
    }

    public Task<FtsSearchEntity> SearchAllAsync(int limit, int offset)
    {
        throw new NotImplementedException();
    }

    public Task IndexAllDocumentsAsync(List<FtsIndexModel> documentss)
    {
        throw new NotImplementedException();
    }

    public Task IndexDocument(FtsIndexModel document)
    {
        throw new NotImplementedException();
    }

    public Task IndexDocumentDataAsync(string data, long docId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDocumentAsync(long docId)
    {
        throw new NotImplementedException();
    }
}