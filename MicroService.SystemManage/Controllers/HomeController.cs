﻿using MicroService.Common;
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
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly IServiceRegistryManage service;
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(ILogger<HomeController> logger,
                              IServiceRegistryManage service,
                              IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.service = service;
            this.httpClientFactory = httpClientFactory;
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

        [HttpGet("getTaskData")]
        public async Task<string> GetTaskData()
        {
            string json = string.Empty;
            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient("SystemManage1");
                HttpResponseMessage response = await httpClient.GetAsync("http://192.168.1.7:10004/WeatherForecast");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    json = await response.Content.ReadAsStringAsync();
                else
                    ConsoleHelper.WriteErrorLine("服务调用报错, msg: " + await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine("服务调用失败, ex: " + ex.Message);
            }
            return json;
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok("ok");
    }
}
