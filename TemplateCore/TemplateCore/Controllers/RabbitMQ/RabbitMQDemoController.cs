using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Demonstration.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateCore.Controllers.RabbitMQ
{
    //[Produces("application/json")]
    [Route("api/RabbitMQDemo")]
    public class RabbitMQDemoController : Controller
    {
        private readonly IRabbitMQDemoService _demoService;
        public RabbitMQDemoController(IRabbitMQDemoService demoService)
        {
            _demoService = demoService;
        }


        [HttpGet("v1/publish/{value}")]
        public void Publish(string value)
        {
            _demoService.Publish(value);
        }
    }
}