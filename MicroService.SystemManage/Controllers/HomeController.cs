using MicroService.Common;
using MicroService.Core;
using MicroService.Core.Authorization;
using MicroService.Core.Consul;
using MicroService.Core.JwtHelper;
using MicroService.SystemManage.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
        private readonly IUserHelper userHelper;
        private readonly IOptions<AuthenticationConfig> options;
        private readonly IOptions<PollyHttpClientConfig> pollyOptions;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger,
                              IServiceRegistryManage service,
                              IHttpClientFactory httpClientFactory,
                              IUserHelper userHelper,
                              IOptions<AuthenticationConfig> options,
                              IOptions<PollyHttpClientConfig> pollyOptions,
                              IConfiguration configuration)
        {
            this.logger = logger;
            this.service = service;
            this.httpClientFactory = httpClientFactory;
            this.userHelper = userHelper;
            this.options = options;
            this.pollyOptions = pollyOptions;
            this.configuration = configuration;
        }

        [HttpGet("testAuth")]
        [Authorize]
        public string TestAuth()
        {
            return userHelper.GetUserID();
        }

        [HttpGet("get")]
        public string Get()
        {
            logger.LogInformation($"【{DateTime.Now}】 MicroService.SystemManage Get1");
            Console.WriteLine(Appsettings.app("Test"));
            return $"【{configuration.GetValue<string>("Test")}】this is SystemManage service1";
        }

        [HttpGet("getService")]
        public async Task<string> GetService()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(await service.GetServices("SystemManage"));
        }

        [HttpGet("error")]
        public string Error()
        {
            Thread.Sleep(2000);
            string a = null;
            return a.ToString();
        }

        [HttpGet("getToken")]
        public string GetToken()
        {
            return JwtHelper.IssueJwt(options.Value, new UserInfo
                                                      {
                                                          UserID = 1,
                                                          UserName = "admin"
                                                      });
        }

        [HttpGet("getTaskData")]
        public async Task<string> GetTaskData()
        {
            string json = string.Empty;
            try
            {
                HttpClient httpClient = httpClientFactory.CreateClient("MicroService.SystemManage");
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
