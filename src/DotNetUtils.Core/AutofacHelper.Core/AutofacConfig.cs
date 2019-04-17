using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dependency.Core;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacHelper.Core
{
    /// <summary>
    /// Autofac配置文件
    /// </summary>
    public class AutofacConfig
    {
        /// <summary>
        /// 容器
        /// </summary>
        public static IContainer Container { set; get; }

        /// <summary>
        /// 负责调用autofac框架实现业务逻辑层和数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数)
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns></returns>
        public static IServiceProvider Register(IServiceCollection services)
        {
            //实例化一个autofac的创建容器
            var builder = new ContainerBuilder();
            // 获取所有相关类库的程序集
            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load);
            var enumerable = assemblies as Assembly[] ?? assemblies.ToArray();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope();
            //管道寄居
            builder.Populate(services);
            //创建一个Autofac的容器
            Container = builder.Build();
            return new AutofacServiceProvider(Container);
        }
    }
}
