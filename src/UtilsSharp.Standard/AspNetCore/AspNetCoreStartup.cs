using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using UtilsSharp;
using UtilsSharp.Dependency;

namespace AspNetCore
{
    /// <summary>
    /// AspNetCoreStartup
    /// </summary>
    public abstract class AspNetCoreStartup
    {
        /// <summary>
        /// 依赖注入映射
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            Init(builder);
        }

        /// <summary>
        /// 依赖注入初始化 [Intercept(typeof(LoggerInterceptor))]
        /// </summary>
        /// <param name="builder">ContainerBuilder</param>
        public void Init(ContainerBuilder builder)
        {
            //获取所有相关类库的程序集
            var enumerable = AssemblyHelper.GetAllAssemblies();
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors();//注册类
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// 依赖注入初始化
        /// </summary>
        /// <typeparam name="T">IInterceptor 拦截器</typeparam>
        /// <param name="builder">ContainerBuilder</param>
        public void Init<T>(ContainerBuilder builder) where T : class, IInterceptor
        {
            //获取所有相关类库的程序集
            var enumerable = AssemblyHelper.GetAllAssemblies();
            builder.RegisterType<T>();//注册拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        }
    }
}
