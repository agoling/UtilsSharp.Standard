using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extras.DynamicProxy;
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

        public static void Register()
        {
            var builder = new ContainerBuilder();
            var enumerables = AssemblyHelper.GetAllAssemblies();
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors();//注册类
            _lifetimeScope = builder.Build();
        }

        public static void Register(Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();
            var enumerables = AssemblyHelper.GetAllAssemblies();
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ISingletonDependency).IsAssignableFrom(t) && typeof(ISingletonDependency) != t).AsImplementedInterfaces().SingleInstance().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && typeof(ITransientDependency) != t).AsImplementedInterfaces().InstancePerDependency().EnableInterfaceInterceptors();//注册类
            builder.RegisterAssemblyTypes(enumerables.ToArray()).Where(t => typeof(IUnitOfWorkDependency).IsAssignableFrom(t) && typeof(IUnitOfWorkDependency) != t).AsImplementedInterfaces().InstancePerLifetimeScope().EnableInterfaceInterceptors();//注册类
            action(builder);
            _lifetimeScope = builder.Build();
        }
    }
}
