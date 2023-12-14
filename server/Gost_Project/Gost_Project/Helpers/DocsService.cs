using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Fields;
using Gost_Project.Data.Repositories.Docs;

namespace Gost_Project.Helpers;

public class DocsService : IDocsService
{
    private readonly IDocsRepository _docsRepository;
    private readonly IFieldsRepository _fieldsRepository;

    public DocsService(IDocsRepository docsRepository, IFieldsRepository fieldsRepository)
    {
        _fieldsRepository = fieldsRepository;
        _docsRepository = docsRepository;
    }

    public long AddNewDoc(FieldEntity primaryField)
    {
        var actualField = new FieldEntity();

        var primaryId = _fieldsRepository.Add(primaryField);
        var actualId = _fieldsRepository.Add(actualField);

        var doc = new DocEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };
        var docId = _docsRepository.Add(doc);

        return docId;
    }
}