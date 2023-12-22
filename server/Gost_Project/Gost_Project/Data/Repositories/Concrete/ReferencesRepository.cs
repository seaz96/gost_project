using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;

namespace Gost_Project.Data.Repositories.Concrete;

public class ReferencesRepository(DataContext context) : IReferencesRepository
{
    private readonly DataContext _context = context;

    public List<DocReferenceEntity> GetAll()
    {
        return _context.DocsReferences.ToList();
    }

    public DocReferenceEntity GetById(long id)
    {
        return _context.DocsReferences.Where(reference => reference.Id == id).FirstOrDefault();
    }

    public DocReferenceEntity GetByParentId(long id)
    {
        return _context.DocsReferences.FirstOrDefault(reference => reference.ParentalDocId == id);
    }

    public DocReferenceEntity GetByChildId(long id)
    {
        return _context.DocsReferences.FirstOrDefault(reference => reference.ChildDocId == id);
    }

    public void Add(DocReferenceEntity reference)
    {
        _context.DocsReferences.Add(reference);
    }
}