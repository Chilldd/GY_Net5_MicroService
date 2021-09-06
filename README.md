﻿﻿## 功能模块:  
- [x] 服务注册(Consul):  
	  服务启动自动注册，服务关闭时自动下线
	  注册，下线，获取服务: IServiceRegistryManage
- [x] 网关(Ocelot):  
	  Consul集成  
	  服务调用负载均衡  
	  限流  
	  熔断  
	  认证  
	  聚合所有微服务Swagger  
	  与配置中心集成  
- [x] 容错(Http):  
	  熔断  
	  重试  
	  超时  
	  资源隔离  
	  与配置中心集成(通过在appsetting中配置)  

- [x] 配置中心:  
      使用Consul作为配置中心，可区分生产环境测试环境等
- [ ] 日志中心(ELK集成):  
- [x] 链路监控(Skywalking):  
      链路监控(初版)
	  请求会话日志聚合(未完成)
- [x] 事件总线(CAP):  
	  RabbitMQ作为消息中间件
	  Mysql作为消息存储
	    

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

## 命名规范:  
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



## 模块截图:  


<h3>配置中心：</h3>

1. 文件夹：

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182410193.png?raw=true" width="100%" />

2. 配置文件

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182204574.png?raw=true" width="100%" />

3. 配置详情

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182352335.png?raw=true" width="100%" />


<hr />


<h3>注册中心</h3>

1. 所有实例

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182609304.png?raw=true" width="100%" />

2. 节点详情

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182646610.png?raw=true" width="100%" />


<hr />


<h3>网关</h3>

1. 聚合swagger文档

<img src="https://github.com/Chilldd/GY_Net5_MicroService/blob/master/img/image-20210830182856554.png?raw=true" width="100%" />
