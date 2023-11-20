using Gost_Project.Data.Context;
using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories;

public class ReferencesRepository : IReferencesRepository
{
    private readonly DataContext _context;

    public ReferencesRepository(DataContext context)
    {
        _context = context;
    }

    public List<NormativeReferenceEntity> GetAll()
    {
        return _context.NormativeReferences.ToList();
    }

    public NormativeReferenceEntity GetById(long id)
    {
        return _context.NormativeReferences.Where(reference => reference.Id == id).FirstOrDefault();
    }

    public NormativeReferenceEntity GetByParentId(long id)
    {
        return _context.NormativeReferences.FirstOrDefault(reference => reference.ParentalGostId == id);
    }
    
    public NormativeReferenceEntity GetByChildId(long id)
    {
        return _context.NormativeReferences.FirstOrDefault(reference => reference.ChildGostId == id);
    }

    public void Add(NormativeReferenceEntity reference)
    {
        _context.NormativeReferences.Add(reference);
    }
}