﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core22SwaggerWebApp.Controllers.v0
{
    public class VZeroGetViewModel
    {
        public string UniqueCode { get; set; } = Guid.NewGuid().ToString();
    }

    [Route("api/[controller]")]
    [ApiController]
    public class VZeroController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<VZeroGetViewModel>> Get()
        {
            return new List<VZeroGetViewModel>
            {
                new VZeroGetViewModel()
            };
        }

        //[HttpGet]
        ////[Obsolete]
        //public ActionResult<VZeroGetViewModel> Get(int id)
        //{
        //    return new VZeroGetViewModel();
        //}
    }
}
