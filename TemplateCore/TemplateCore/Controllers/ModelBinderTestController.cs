using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using TemplateCore.Model;

namespace TemplateCore.Controllers
{
    [Produces("application/json")]
    [Route("api/ModelBinderTest")]
    public class ModelBinderTestController : Controller
    {
        [HttpPost("GetFromData")]
        public async Task<JsonResult> GetFromData([FromForm]GetFormDataInput input)
        {
            return await Task.FromResult(Json(input));
        }

        

    }
}