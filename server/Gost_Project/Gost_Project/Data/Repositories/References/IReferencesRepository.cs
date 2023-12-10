using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.References;

public interface IReferencesRepository
{
    public List<NormativeReferenceEntity> GetAll();

    public NormativeReferenceEntity GetById(long id);

    public NormativeReferenceEntity GetByParentId(long id);

    public NormativeReferenceEntity GetByChildId(long id);

    public void Add(NormativeReferenceEntity reference);
}