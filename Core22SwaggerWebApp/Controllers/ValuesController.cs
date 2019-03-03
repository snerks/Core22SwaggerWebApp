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

        public bool IsDefault { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(
            ILogger<ValuesController> logger,
            //IOptions<CustomSettings> customSettings,
            //IOptions<CurrenciesSettings> currenciesSettings,
            CustomSettings customSettings,
            CurrenciesSettings currenciesSettings)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CustomSettings = customSettings ?? throw new ArgumentNullException(nameof(customSettings));
            CurrenciesSettings = currenciesSettings ?? throw new ArgumentNullException(nameof(currenciesSettings));
            //CurrenciesSettings = currenciesSettings ?? throw new ArgumentNullException(nameof(currenciesSettings));
            //CustomSettings = customSettings ?? throw new ArgumentNullException(nameof(customSettings));
        }

        public ILogger<ValuesController> Logger { get; }
        public CustomSettings CustomSettings { get; }
        public CurrenciesSettings CurrenciesSettings { get; }

        //public IOptions<CurrenciesSettings> CurrenciesSettings { get; }
        //public IOptions<CustomSettings> CustomSettings { get; }

        private readonly Dictionary<string, CurrencyGetViewModel> 
            _isoCodeCurrenciesMap = new Dictionary<string, CurrencyGetViewModel>(StringComparer.CurrentCultureIgnoreCase);

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

            Dictionary<string, CurrencyGetViewModel> currenciesMap = GetIsoCodeCurrenciesMap();

            return currenciesMap.Values;
        }

        private Dictionary<string, CurrencyGetViewModel> GetIsoCodeCurrenciesMap()
        {
            if (_isoCodeCurrenciesMap.Keys.Any())
            {
                return _isoCodeCurrenciesMap;
            }

            ////var customSettings = CustomSettings.Value;
            //var customSettings = CustomSettings;
            //Console.WriteLine(customSettings.Currency.DefaultIsoCode);

            //var currenciesSettings = CurrenciesSettings.Value;
            //var currenciesSettings = CurrenciesSettings;
            //Console.WriteLine(currenciesSettings.DefaultIsoCode);

            //var currenciesSettings = CurrenciesSettings;

            //var isoCodeCurrenciesMap = 
            //    new Dictionary<string, CurrencyGetViewModel>(StringComparer.CurrentCultureIgnoreCase);

            var normalisedDefaultIsoCode = CurrenciesSettings.DefaultIsoCode?.ToUpper().Trim();

            foreach (var currenciesItem in CurrenciesSettings.Currencies)
            {
                try
                {
                    var normalisedIsoCode = currenciesItem.IsoCode?.ToUpper();

                    var currencyGetViewModel = new CurrencyGetViewModel(
                            normalisedIsoCode,
                            currenciesItem.Symbol,
                            currenciesItem.Name);

                    currencyGetViewModel.IsDefault =
                        normalisedIsoCode == normalisedDefaultIsoCode;

                    _isoCodeCurrenciesMap.Add(
                         normalisedIsoCode,
                         currencyGetViewModel);

                    //bool wasAddSuccessful = currenciesMap.TryAdd(
                    //    normalisedIsoCode,
                    //    currencyGetViewModel);
                }
                catch (ArgumentException ex)
                {
                    // throw;
                    Logger.LogWarning("{@Ex}", ex);
                }
            }

            return _isoCodeCurrenciesMap;
        }

        // GET api/values/5
        [HttpGet("{isoCode}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<CurrencyGetViewModel> Get(string isoCode)
        {
            if (isoCode == null)
            {
                return BadRequest();
            }

            var isoCodeCurrenciesMap = GetIsoCodeCurrenciesMap();

            if (isoCodeCurrenciesMap == null)
            {
                return NotFound();
            }

            var key = isoCode.Trim();

            if (isoCodeCurrenciesMap.ContainsKey(key))
            {
                return isoCodeCurrenciesMap[key];
            }

            return NotFound();
        }

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
