using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace AspNet.Core.Extensions.Autofac
{
    /// <summary>
    /// 依赖注入Container
    /// </summary>
    public class AutofacContainer
    {
        private static ILifetimeScope lifetimeScope;

        public static ILifetimeScope Current => lifetimeScope;

        internal static void Register(ILifetimeScope autofacRoot)
        {
           lifetimeScope = autofacRoot;
        }
    }
}
