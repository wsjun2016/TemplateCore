using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateCore.Controllers
{
   // [Produces("application/json")]
    //[Route("api/Home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect($"http://{HttpContext.Request.Host}/swagger");
        }

        public IActionResult Error()
        {
            return Content(Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        }
    }
}