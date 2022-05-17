using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using UtilsSharp;
using UtilsSharp.Dependency;

namespace DotNetCore.Autofac
{
    /// <summary>
    /// 依赖注入Container
    /// </summary>
    public class AutofacContainer
    {
        private static ILifetimeScope _lifetimeScope;

        public static ILifetimeScope Current => _lifetimeScope;

        /// <summary>
        /// 依赖注入初始化
        /// </summary>
        public static void Register()
        {
            var builder = new ContainerBuilder();
            var enumerables = AssemblyHelper.GetAllAssemblies();
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors();//注册类
            builder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(enumerables);
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
            _lifetimeScope = builder.Build();
        }

        /// <summary>
        /// 依赖注入初始化
        /// </summary>
        /// <param name="action">回调</param>
        public static void Register(Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();
            var enumerables = AssemblyHelper.GetAllAssemblies();
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors();//注册类
            builder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(enumerables);
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
            action(builder);
            _lifetimeScope = builder.Build();
        }
    }
}
