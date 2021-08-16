using MicroService.Core.Consul;
using MicroService.SystemManage.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly IServiceRegistryManage service;

        public HomeController(ILogger<HomeController> logger, IServiceRegistryManage service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpGet("get")]
        public string Get()
        {
            logger.LogInformation($"【{DateTime.Now}】 MicroService.SystemManage Get1");
            return "this is SystemManage service1";
        }

        [HttpGet("getService")]
        public async Task<string> GetService()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(await service.GetServices("SystemManage"));
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok("ok");
    }
}
