using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Http;
using UtilsSharp.Dependency;

namespace AspNet.Core.Extensions
{
    /// <summary>
    /// 依赖注入抽象类
    /// </summary>
    public abstract class AutofacStartup
    {
        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="builder"></param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            // 获取所有相关类库的程序集
            var assemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Select(Assembly.Load);
            var enumerable = assemblies as Assembly[] ?? (assemblies ?? throw new InvalidOperationException()).ToArray();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        }
    }
}
