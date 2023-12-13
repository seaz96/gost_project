using Gost_Project.Data.Entities;
using Gost_Project.Data.Models;
using Gost_Project.Data.Repositories.Fields;
using Gost_Project.Data.Repositories.Gosts;

namespace Gost_Project.Helpers;

public class GostsService : IGostsService
{
    private readonly IGostsRepository _gostsRepository;
    private readonly IFieldsRepository _fieldsRepository;

    public GostsService(IGostsRepository gostsRepository, IFieldsRepository fieldsRepository)
    {
        _fieldsRepository = fieldsRepository;
        _gostsRepository = gostsRepository;
    }

    public long AddNewGost(FieldEntity primaryField)
    {
        var actualField = new FieldEntity();

        var primaryId = _fieldsRepository.Add(primaryField);
        var actualId = _fieldsRepository.Add(actualField);

        var gost = new GostEntity { ActualFieldId = actualId, PrimaryFieldId = primaryId };
        var gostId = _gostsRepository.Add(gost);

        return gostId;
    }
}