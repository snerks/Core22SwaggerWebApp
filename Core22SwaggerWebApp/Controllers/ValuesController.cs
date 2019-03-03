using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Core22SwaggerWebApp.Controllers
{
    // Match Property Names with Config Key Names
    public class CustomSettings
    {
        public CurrenciesSettings Currency { get; set; } = new CurrenciesSettings();
    }

    public class CurrenciesSettings
    {
        public string DefaultIsoCode { get; set; }

        //[JsonProperty(PropertyName = "Currencies")]
        public List<CurrencySettings> Currencies { get; set; } = new List<CurrencySettings>();
    }

    public class CurrencySettings
    {
        public string IsoCode { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }
    }

    public class CurrencyService
    {
        public IEnumerable<CurrencyGetViewModel> GetAll()
        {
            return new List<CurrencyGetViewModel>();
        }
    }

    public class CurrencyGetViewModel
    {
        public CurrencyGetViewModel(string isoCode, string symbol, string name)
        {
            IsoCode = isoCode ?? throw new ArgumentNullException(nameof(isoCode));
            Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string IsoCode { get; }

        public string Symbol { get; }

        public string Name { get; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(
            ILogger<ValuesController> logger,
            IOptions<CustomSettings> customSettings,
            IOptions<CurrenciesSettings> currenciesSettings)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CurrenciesSettings = currenciesSettings ?? throw new ArgumentNullException(nameof(currenciesSettings));
            CustomSettings = customSettings ?? throw new ArgumentNullException(nameof(customSettings));
        }

        public ILogger<ValuesController> Logger { get; }
        public IOptions<CurrenciesSettings> CurrenciesSettings { get; }
        public IOptions<CustomSettings> CustomSettings { get; }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<CurrencyGetViewModel>> Get()
        {
            // var currenciesMap = new Dictionary<string, CurrencyGetViewModel>
            // {
            //     ["GBP"] = new CurrencyGetViewModel("GBP", "£", "Pound sterling"),
            //     ["EUR"] = new CurrencyGetViewModel("EUR", "€", "Euro"),
            //     ["USD"] = new CurrencyGetViewModel("USD", "$", "United States dollar"),
            //     ["AUD"] = new CurrencyGetViewModel("AUD", "$", "Australian dollar"),
            //     ["ABC"] = new CurrencyGetViewModel("ABC", "$", "ABC dollar"),
            //     ["DEF"] = new CurrencyGetViewModel("DEF", "$", "DEF dollar"),
            // };

            // foreach (var key in currenciesMap.Keys)
            // {
            //     Logger.LogInformation("In my wallet I have {Key}: {@Currency}", key, currenciesMap[key]);
            // }

            // return currenciesMap.Values;

            //var items = 
            //    CurrenciesSettings
            //    .GetSection("CustomSettings:Currency:Currencies")
            //    .Get<List<CurrencySettings>>();

            var customSettings = CustomSettings.Value;
            Console.WriteLine(customSettings.Currency.DefaultIsoCode);

            var currenciesSettings = CurrenciesSettings.Value;
            Console.WriteLine(currenciesSettings.DefaultIsoCode);

            var currenciesMap = new Dictionary<string, CurrencyGetViewModel>();

            foreach (var currenciesItem in currenciesSettings.Currencies)
            {
                try
                {
                    currenciesMap.Add(
                        currenciesItem.IsoCode,
                        new CurrencyGetViewModel(
                            currenciesItem.IsoCode,
                            currenciesItem.Symbol,
                            currenciesItem.Name));
                }
                catch (ArgumentException ex)
                {
                    // throw;
                    Logger.LogWarning("{@Ex}", ex);
                }
            }

            return currenciesMap.Values;
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
