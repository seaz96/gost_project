using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Gost_Project.Data.Repositories.Concrete;
using Gost_Project.Services.Abstract;

namespace Gost_Project.Services.Concrete;

public class FieldsService(IFieldsRepository fieldsRepository, IReferencesRepository referencesRepository, IDocsRepository docsRepository) : IFieldsService
{
    private readonly IFieldsRepository _fieldsRepository = fieldsRepository;
    private readonly IReferencesRepository _referencesRepository = referencesRepository;
    private readonly IDocsRepository _docsRepository = docsRepository;

    public void Update(FieldEntity updatedEntity, long docId)
    {
        var doc = _docsRepository.GetById(docId);
        updatedEntity.Id = doc.PrimaryFieldId;
        _fieldsRepository.Update(updatedEntity);
    }
}