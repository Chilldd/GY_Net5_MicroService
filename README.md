﻿
功能模块:  
- [x] 服务注册(Consul):  
	  服务启动自动注册，服务关闭时自动下线
	  注册，下线，获取服务: IServiceRegistryManage
- [x] Log:  
	  Log4net: 记录物理日志  
	  ELK: 待实现  
- [x] 身份验证:  
	  jwt方案: 认证，授权，获取用户信息 (ioc注入：IUserHelper)  
- [x] IOC，AOP:  
	  Autofac: 默认读取当前项目下所有的class注入 (配置文件中需要配置项目名称，可通过NotAOP，NotInject设置不注入，不走切面)  
	  AOP:  
		  日志切面: 通过配置文件控制是否开启, 方法需要是虚方法  
		  事务切面: 通过配置文件控制是否开启, 方法需要是虚方法，并且添加特性[UseTran]  
- [x] ORM: SqlSugar:  
	  sql日志, 泛型仓储  
- [x] Redis:  
	  注入: IRedisBasketRepository  
- [x] 全局异常处理:  
	  采用Filter拦截  
- [x] 实体映射(AutoMapper):  
	  项目下新建一个Mappers文件夹，继承Profile，在构造函数里创建映射关系即可

项目命名规范:  
- [x] 类名: 驼峰，首字母大写  
      case: UserService, UserHelper  
- [x] 方法/属性: 驼峰，首字母大写  
      case: GetUser, ListUser  
- [x] 字段: 驼峰，首字母小写  
      case: userService, userHelper  
- [x] 枚举: 驼峰，首字母大写，必须以Enum结尾  
      case: UserStatusEnum  
	  枚举项: 驼峰，首字母大写  
      case: Enable, Disable  
- [x] 常量: 驼峰，首字母大写(部分常量使用下划线间隔)  
      case: ConsulConfigName, RedisConfigName, Claim_UserID, Claim_UserName  
