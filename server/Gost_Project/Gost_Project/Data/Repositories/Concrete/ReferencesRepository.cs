using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

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

    public DocReferenceEntity? GetByParentId(long id)
    {
        return _context.DocsReferences.FirstOrDefault(reference => reference.ParentalDocId == id);
    }

    public DocReferenceEntity? GetByChildId(long id)
    {
        return _context.DocsReferences.FirstOrDefault(reference => reference.ChildDocId == id);
    }

    public void Add(DocReferenceEntity reference)
    {
        _context.DocsReferences.Add(reference);
        _context.SaveChanges();
    }

    public void AddRange(List<DocReferenceEntity> references)
    {
        _context.DocsReferences.AddRange(references);
        _context.SaveChanges();
    }

    public void DeleteAllByParentId(long parentId)
    {
        _context.DocsReferences.Where(reference => reference.ParentalDocId == parentId).ExecuteDelete();
        _context.SaveChanges();
    }
    
    public void DeleteAllByChildId(long parentId)
    {
        _context.DocsReferences.Where(reference => reference.ChildDocId == parentId).ExecuteDelete();
        _context.SaveChanges();
    }

    public void UpdateByParentId(List<long> referenceIds, long parentId)
    {
        var references = _context.DocsReferences.Where(reference => reference.ParentalDocId == parentId);
        var toDelete = new List<long>();
        
        foreach (var reference in references)
        {
            if (referenceIds.Contains(reference.ChildDocId))
            {
                referenceIds.Remove(reference.ChildDocId);
            }
            else
            {
                referenceIds.Remove(reference.ChildDocId);
                toDelete.Add(reference.ChildDocId);
            }
        }

        _context.DocsReferences.Where(reference =>
            reference.ParentalDocId == parentId && toDelete.Contains(reference.ChildDocId)).ExecuteDelete();
        
        AddRange(referenceIds.Select(id => new DocReferenceEntity {ChildDocId = id, ParentalDocId = parentId}).ToList());
    }
}