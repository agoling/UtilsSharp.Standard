﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using UtilsSharp.AsyncInterceptor;
using UtilsSharp.Shared.Dependency;

namespace UtilsSharp.AspNetCore
{
    /// <summary>
    /// AutofacStartup
    /// </summary>
    public abstract class AutofacStartup
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
            //注册AutoMapping
            builder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(enumerable);
                }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
                {
                    //This resolves a new context that can be used later.
                    var context = c.Resolve<IComponentContext>();
                    var config = context.Resolve<MapperConfiguration>();
                    return config.CreateMapper(context.Resolve);
                })
                .As<IMapper>()
                .InstancePerLifetimeScope();
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
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IAsyncInterceptor).IsAssignableFrom(t) && typeof(IAsyncInterceptor) != t && !t.IsAbstract).AsSelf();//注册异步拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterAssemblyTypes(enumerable.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().InterceptedBy(typeof(T)).EnableInterfaceInterceptors();//注册类并为其添加拦截器
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            //注册AutoMapping
            builder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(enumerable);
                }
            )).AsSelf().SingleInstance();

            builder.Register(c =>
                {
                    //This resolves a new context that can be used later.
                    var context = c.Resolve<IComponentContext>();
                    var config = context.Resolve<MapperConfiguration>();
                    return config.CreateMapper(context.Resolve);
                })
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}
