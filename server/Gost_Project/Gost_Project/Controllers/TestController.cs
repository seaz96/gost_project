using Gost_Project.Data;
using Gost_Project.Data.Entities;
using Gost_Project.Data.Repositories.Fields;
using Gost_Project.Data.Repositories.Gosts;
using Microsoft.AspNetCore.Mvc;

namespace Gost_Project.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IFieldsRepository _fieldsRepository;
    private readonly IGostsRepository _gostsRepository;

    public TestController(DataContext context, IGostsRepository gostsRepository,
        IFieldsRepository fieldsRepository)
    {  
        _context = context;
        _gostsRepository = gostsRepository;
        _fieldsRepository = fieldsRepository;
    }
    
    [HttpPost("test")]
    public async Task<IActionResult> Test(FieldEntity field)
    {
        var newField = new FieldEntity();
        _gostsRepository.Add(new GostEntity() { Id = 3, ActualFieldId = 5, PrimaryFieldId = 6 });
        var gost = _gostsRepository.GetById(1);
        _fieldsRepository.Add(new FieldEntity { Id = 5 });
        _fieldsRepository.Add(new FieldEntity { Id = 6 });
        var actualField = _fieldsRepository.GetById(gost.ActualFieldId);
        var primaryField = _fieldsRepository.GetById(gost.PrimaryFieldId);

        return Ok();
    }
}