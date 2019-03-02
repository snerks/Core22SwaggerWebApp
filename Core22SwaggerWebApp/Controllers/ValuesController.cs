using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core22SwaggerWebApp.Controllers
{
    public class MyInfo
    {
        public int Id { get; set; }

        public string MyString { get; set; }

        public DateTime MyDateTime { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(ILogger<ValuesController> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ILogger<ValuesController> Logger { get; }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var result = new string[] { "value1", "value2" };

            //Logger.LogDebug("{result}");
            //Logger.LogInformation("Result was: {@result}", result);

            var position = new { Latitude = 25, Longitude = 134 };
            var elapsedMs = 34;

            Logger.LogInformation("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
            Logger.LogInformation("Returned {@Result}", result.ToList());

            var myInfo = new MyInfo
            {
                Id = 88,
                MyDateTime = DateTime.UtcNow,
                MyString = "Sample MyString"
            };

            Logger.LogInformation("Returned {@MyInfo}", myInfo);

            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Get(int id)
        {
            if (id == 99)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "value must not be 99");
            }

            return "value";

            // return NotFound();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
