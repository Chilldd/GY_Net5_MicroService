
using AutoMapper;
using MicroService.Core;
using MicroService.Core.ORM;
using MicroService.SystemManage.Model.Entity;
using MicroService.SystemManage.Model.VO;
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
    [Route("api/[controller]")]
    [ApiController]
    public class UserManage : ControllerBase
    {
        private readonly UserManageService service;
        private readonly IMapper mapper;

        public UserManage(UserManageService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<ResultEntity<UserInfoVO>> ListData()
        {
            UserEntity user = new UserEntity
            {
                Account = "1",
                Password = "1",
                Name = "1"
            };
            var res = mapper.Map<UserEntity, UserInfoVO>(user);
            return new ResultEntity<UserInfoVO>(ResultEnum.Success, res);
        }

        [HttpPost("get")]
        public virtual async Task<UserEntity> Get(int id)
        {
            return null;
        }

        [HttpPost("add")]
        public virtual async Task<UserEntity> Add(UserEntity entity)
        {
            return null;
        }

        [HttpPost("update")]
        public virtual async Task<UserEntity> Update(UserEntity entity)
        {
            return null;
        }

        [HttpPost("delete")]
        public virtual async Task<UserEntity> Delete(UserEntity entity)
        {
            return null;
        }

        [HttpPost("resetPassword")]
        public virtual async Task<UserEntity> ResetPassword(UserEntity entity)
        {
            return null;
        }
    }
}
