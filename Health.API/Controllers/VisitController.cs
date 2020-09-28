using Health.API.Infra;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers
{
    [ApiController]
    [Route("Patient/[controller]")]
    [AuthorizeUser]
    public class VisitController : ControllerBase
    {
        [HttpGet]
        [Route("summary")]
        public IActionResult VisitSummary()
        {
            return Ok(new {
                MRN = "19309",
                Name= "Patient A",
                ICD = "J09.X2",
                Status="Resolved 27-09-2019"
            });
        }

    }
}
