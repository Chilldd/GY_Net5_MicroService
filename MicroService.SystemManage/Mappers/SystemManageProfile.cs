using AutoMapper;
using MicroService.SystemManage.Model.Entity;
using MicroService.SystemManage.Model.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.SystemManage.Mappers
{
    public class SystemManageProfile : Profile
    {
        public SystemManageProfile()
        {
            CreateMap<UserEntity, UserInfoVO>().ReverseMap();
        }
    }
}
