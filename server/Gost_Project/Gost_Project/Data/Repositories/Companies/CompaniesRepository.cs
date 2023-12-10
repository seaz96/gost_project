using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories.Companies;

public class CompaniesRepository : ICompaniesRepository
{
    private readonly DataContext _context;

    public CompaniesRepository(DataContext context)
    {
        _context = context;
    }

    public List<CompanyEntity> GetAll()
    {
        return _context.Companies.ToList();
    }

    public CompanyEntity GetById(long id)
    {
        return _context.Companies.Where(company => company.Id == id).FirstOrDefault();
    }

    public void Add(CompanyEntity company)
    {
        _context.Companies.Add(company);
    }

    public void Delete(long id)
    {
        var company = _context.Companies.FirstOrDefault(company => company.Id == id);
        if (company is not null)
        {
            _context.Companies.Remove(company);
        }
    }
}