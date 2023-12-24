using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Abstract;

public interface IReferencesRepository
{
    public List<DocReferenceEntity> GetAll();

    public DocReferenceEntity GetById(long id);

    public DocReferenceEntity GetByParentId(long id);

    public DocReferenceEntity GetByChildId(long id);

    public void Add(DocReferenceEntity reference);

    public void AddRange(List<DocReferenceEntity> references);

    public void DeleteAllByParentId(long parentId);

    public void DeleteAllByChildId(long parentId);

    public void UpdateByParentId(List<long> referenceIds, long parentId);
}