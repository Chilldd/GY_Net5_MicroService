using DotNetCore.CAP;
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
        private readonly ICapPublisher capPublisher;

        public HomeController(ILogger<HomeController> logger,
                              IServiceRegistryManage service,
                              IHttpClientFactory httpClientFactory,
                              IUserHelper userHelper,
                              IOptions<AuthenticationConfig> options,
                              IOptions<PollyHttpClientConfig> pollyOptions,
                              IConfiguration configuration,
                              ICapPublisher capPublisher)
        {
            this.logger = logger;
            this.service = service;
            this.httpClientFactory = httpClientFactory;
            this.userHelper = userHelper;
            this.options = options;
            this.pollyOptions = pollyOptions;
            this.configuration = configuration;
            this.capPublisher = capPublisher;
        }

        [HttpGet("testAuth")]
        [Authorize]
        public string TestAuth()
        {
            logger.LogInformation($"【{DateTime.Now}】 MicroService.SystemManage TestAuth{userHelper.GetUserID()}");
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
                var list = await service.GetServices("TaskCenter");
                var serviceInfo = list.FirstOrDefault();
                string url = $"http://{serviceInfo.Address}:{serviceInfo.Port}/WeatherForecast/get";
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    json = await response.Content.ReadAsStringAsync();
                else
                    logger.LogInformation("【SystemManage】服务调用报错, msg: " + await response.Content.ReadAsStringAsync());

                logger.LogInformation("【SystemManage】服务调用成功");
            }
            catch (Exception ex)
            {
                logger.LogInformation("【SystemManage】服务调用失败, ex: " + ex.Message);
            }
            return json;
        }

        [HttpGet("testPushMessage")]
        public void TestPushMessage()
        {
            capPublisher.Publish(EventBusConstants.SystemTest, $"发送时间: 【{DateTime.Now}】");
        }

        [HttpGet("testGC")]
        public void TestGC()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                string a = "";
                for (int j = 0; j < 100000; j++)
                {
                    a += j.ToString();
                }
            }
        }

        [HttpGet("health")]
        public IActionResult Health() => Ok("ok");
    }
}
