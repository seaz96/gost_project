using Gost_Project.Data.Context;
using Gost_Project.Data.Entities;

namespace Gost_Project.Data.Repositories;

public interface ICompaniesRepository
{
    public List<CompanyEntity> GetAll();
    
    public CompanyEntity GetById(long id);
    
    public void Add(CompanyEntity company);
    
    public void Delete(long id);
}