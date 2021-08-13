using MicroService.Core;
using MicroService.Core.ORM;
using MicroService.SystemManage.Model.Entity;
using MicroService.SystemManage.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserManage : ControllerBase
    {
        private readonly UserManageService service;

        public UserManage(UserManageService service)
        {
            this.service = service;
        }

        [HttpGet("list")]
        public async Task<List<UserEntity>> ListData()
        {
            return await service.ListData();
        }

        [HttpPost("add")]
        public virtual async Task<UserEntity> Add(UserEntity entity)
        {
            return await service.Add(entity);
        }
    }
}
