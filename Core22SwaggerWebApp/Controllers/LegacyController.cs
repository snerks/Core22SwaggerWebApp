using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core22SwaggerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Obsolete(message: "Please Use VOne.Get() instead.")]
        public ActionResult<IEnumerable<object>> Get()
        {
            return new List<object>();
        }
    }
}
