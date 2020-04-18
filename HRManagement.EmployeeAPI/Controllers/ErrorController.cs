using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace HRManagement.EmployeeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        [HttpHead]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error() => Problem();
    }

}
