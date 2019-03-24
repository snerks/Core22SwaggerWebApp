using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core22SwaggerWebApp.Controllers
{
    public class SampleComplexTypeViewModel
    {
        public string UniqueId { get; set; }

        public DateTime RelevantDate { get; set; }

        public string ItemUniqueId { get; set; }

        public string ItemQualifierUniqueId { get; set; }

        public bool SampleFlag { get; set; }
    }

    public class SampleComplexTypeGetRequestTwo
    {
        public string UniqueId { get; set; }

        public DateTime RelevantDate { get; set; }
    }

    public class SampleComplexTypeGetRequestThree : SampleComplexTypeGetRequestTwo
    {
        public string ItemUniqueId { get; set; }
    }

    public class SampleComplexTypeGetRequestFour : SampleComplexTypeGetRequestThree
    {
        public string ItemQualifierUniqueId { get; set; }
    }

    public class SampleComplexTypeGetRequestFive : SampleComplexTypeGetRequestFour
    {
        public bool SampleFlag { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ComplexTypesController : ControllerBase
    {
        [HttpGet("{uniqueId}/{relevantDate}")]
        public ActionResult<IEnumerable<SampleComplexTypeViewModel>> GetAll([FromRoute] SampleComplexTypeGetRequestTwo request)
        {
            return GetAllCore(new SampleComplexTypeGetRequestFour
            {
                UniqueId = request.UniqueId,
                RelevantDate = request.RelevantDate,
                //ItemUniqueId = request.ItemUniqueId,
                //ItemQualifierUniqueId = request.ItemQualifierUniqueId,
                //SampleFlag = request.SampleFlag
            });
        }

        [HttpGet("{uniqueId}/{relevantDate}/{itemUniqueId}")]
        public ActionResult<IEnumerable<SampleComplexTypeViewModel>> GetAll([FromRoute] SampleComplexTypeGetRequestThree request)
        {
            return GetAllCore(new SampleComplexTypeGetRequestFour
            {
                UniqueId = request.UniqueId,
                RelevantDate = request.RelevantDate,
                ItemUniqueId = request.ItemUniqueId,
                //ItemQualifierUniqueId = request.ItemQualifierUniqueId,
                //SampleFlag = request.SampleFlag
            });
        }

        [HttpGet("{uniqueId}/{relevantDate}/{itemUniqueId}/{itemQualifierUniqueId}")]
        public ActionResult<IEnumerable<SampleComplexTypeViewModel>> GetAll([FromRoute] SampleComplexTypeGetRequestFour request)
        {
            return GetAllCore(request);
        }

        private ActionResult<IEnumerable<SampleComplexTypeViewModel>> GetAllCore(SampleComplexTypeGetRequestFour request)
        {
            var result = new List<SampleComplexTypeViewModel>
            {
                new SampleComplexTypeViewModel
                {
                    UniqueId = request.UniqueId,
                    RelevantDate = request.RelevantDate,
                    ItemUniqueId = request.ItemUniqueId,
                    ItemQualifierUniqueId = request.ItemQualifierUniqueId,
                    //SampleFlag = request.SampleFlag
                }
            };

            return result;
        }

        [HttpGet("{uniqueId}/{relevantDate}/{itemUniqueId}/{itemQualifierUniqueId}/{sampleFlag}")]
        public ActionResult<SampleComplexTypeViewModel> Get([FromRoute] SampleComplexTypeGetRequestFive request)
        {
            var result = new SampleComplexTypeViewModel
            {
                UniqueId = request.UniqueId,
                RelevantDate = request.RelevantDate,
                ItemUniqueId = request.ItemUniqueId,
                ItemQualifierUniqueId = request.ItemQualifierUniqueId,
                SampleFlag = request.SampleFlag
            };

            return result;
        }
    }
}
