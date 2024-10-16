using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GostStorage.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/heisenberg")]
public class HeisenbergController
{
    [HttpPost("sigma")]
    public Task<IActionResult> Sigma(SigmaModel model)
    {
        throw new Exception(model.Message);
    }

    public class SigmaModel
    {
        public string Message { get; set; }
        public int Count { get; set; }
    }
}