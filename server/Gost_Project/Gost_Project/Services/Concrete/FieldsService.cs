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

    public void Update(FieldEntity updatedField, long docId)
    {
        var doc = _docsRepository.GetById(docId);
        updatedField.Id = doc.PrimaryFieldId;
        _fieldsRepository.Update(updatedField);
    }

    public void Actualize(FieldEntity actualizedField, long docId)
    {
        var doc = _docsRepository.GetById(docId);
        actualizedField.Id = doc.ActualFieldId.Value;
        
        _fieldsRepository.Update(actualizedField);
    }
}