using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core22SwaggerWebApp.Infrastructure;
using Core22SwaggerWebApp.Models;
using Core22SwaggerWebApp.Models.Core22SwaggerWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core22SwaggerWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        public CurrenciesController(
            ILogger<CurrenciesController> logger,
            CustomSettings customSettings,
            CurrenciesSettings currenciesSettings)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CustomSettings = customSettings ?? throw new ArgumentNullException(nameof(customSettings));
            CurrenciesSettings = currenciesSettings ?? throw new ArgumentNullException(nameof(currenciesSettings));
        }

        public ILogger<CurrenciesController> Logger { get; }
        public CustomSettings CustomSettings { get; }
        public CurrenciesSettings CurrenciesSettings { get; }

        public Dictionary<string, CurrencyGetViewModel> IsoCodeCurrenciesMap { get; } =
            new Dictionary<string, CurrencyGetViewModel>(StringComparer.CurrentCultureIgnoreCase);

        [HttpGet()]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        //[ProducesDefaultResponseType]
        //[ValidateModel]
        //[ProducesResponseType(200, Type = typeof(PagedList<CurrencyGetViewModel>))]
        public ActionResult<PagedList<CurrencyGetViewModel>> GetAll([FromQuery] PagingOptions pagingOptions)
        {
            //if (!ModelState.IsValid)
            //{
            //    return new BadRequestObjectResult(new ApiError(ModelState));
            //}

            //var context = new ValidationContext(pagingOptions, serviceProvider: null, items: null);
            //var validationResults = new List<ValidationResult>();

            //bool isValid = Validator.TryValidateObject(pagingOptions, context, validationResults, true);

            Logger.LogDebug("Getting one page of items");

            var currenciesMap = GetIsoCodeCurrenciesMap();
            var currenciesMapValues = currenciesMap.Values;

            //pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            //pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var result =
                new PagedList<CurrencyGetViewModel>(
                        currenciesMapValues.AsQueryable(),
                        pagingOptions.PageNumber,
                        pagingOptions.PageSize);

            return result;
        }

        //// GET api/values/5
        //[HttpGet("{isoCode}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public ActionResult<CurrencyGetViewModel> Get(string isoCode)
        //{
        //    if (isoCode == null)
        //    {
        //        return BadRequest();
        //    }

        //    var isoCodeCurrenciesMap = GetIsoCodeCurrenciesMap();

        //    if (isoCodeCurrenciesMap == null)
        //    {
        //        return NotFound();
        //    }

        //    var key = isoCode.Trim();

        //    if (isoCodeCurrenciesMap.ContainsKey(key))
        //    {
        //        return isoCodeCurrenciesMap[key];
        //    }

        //    return NotFound();
        //}

        private Dictionary<string, CurrencyGetViewModel> GetIsoCodeCurrenciesMap()
        {
            if (IsoCodeCurrenciesMap.Keys.Count > 0)
            {
                return IsoCodeCurrenciesMap;
            }

            var normalisedDefaultIsoCode = CurrenciesSettings.DefaultIsoCode?.ToUpper().Trim();

            foreach (var currenciesItem in CurrenciesSettings.Currencies)
            {
                try
                {
                    var normalisedIsoCode = currenciesItem.IsoCode?.ToUpper().Trim();

                    var currencyGetViewModel = new CurrencyGetViewModel(
                            normalisedIsoCode,
                            currenciesItem.Symbol,
                            currenciesItem.Name)
                    {
                        IsDefault =
                        normalisedIsoCode == normalisedDefaultIsoCode
                    };

                    IsoCodeCurrenciesMap.Add(
                         normalisedIsoCode,
                         currencyGetViewModel);
                }
                catch (ArgumentException ex)
                {
                    Logger.LogWarning("{@Ex}", ex);
                }
            }

            return IsoCodeCurrenciesMap;
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
