using MicroService.Core;
using MicroService.SystemManage.Model.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroService.Core.Redis;

namespace MicroService.SystemManage.Service
{
    public class UserManageService
    {
        private readonly BaseRepository<UserEntity> repository;

        public UserManageService(BaseRepository<UserEntity> repository)
        {
            this.repository = repository;
        }
        public async Task<List<UserEntity>> ListData()
        {
            return await repository.GetListAsync();
        }

        [UseTran]
        public virtual async Task<UserEntity> Add(UserEntity entity)
        {
            await repository.InsertAsync(entity);
            return entity;
        }
    }
}
