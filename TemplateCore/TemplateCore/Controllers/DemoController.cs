using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using BasisFrameWork.Extension;
using BasisFrameWork.Utilities.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateCore.Controllers
{
    //[Produces("application/json")]
    [Route("api/Demo")]
    public class DemoController : Controller
    {
        private readonly IDemoService _dictionaryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DemoController(IDemoService dictionaryService, IHostingEnvironment hostingEnvironment) {
            _dictionaryService = dictionaryService;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET api/values
        [HttpGet("v1/get")]
        public async Task<string> Get() {
            //var data = Utility.Current.Request.Host;
            return await _dictionaryService.Get();
        }

        // GET api/values
        [HttpGet("v1/set/{value}")]
        public async Task<bool> Set(string value) {
            // var data = Utility.Current.Request.Host;
            return await _dictionaryService.Set(value);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get2(int id) {
            return await HttpHelper.GetOnStringAsync("http://www.baidu.com");
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value) {
            string imgUrl = "http://pic14.nipic.com/20110605/1369025_165540642000_2.jpg";
            imgUrl.DownLoadFileAsync($"{_hostingEnvironment.WebRootPath}//img.jpg", it =>
            {

            });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}