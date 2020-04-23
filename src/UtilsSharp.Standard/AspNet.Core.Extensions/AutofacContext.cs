using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace AspNet.Core.Extensions
{
    /// <summary>
    /// 依赖注入上下文
    /// </summary>
    public static class AutofacContext
    {
        public static IApplicationBuilder UseAutofacContext(this IApplicationBuilder app)
        {
            ILifetimeScope autofacRoot = app.ApplicationServices.GetAutofacRoot();
            AutofacContainer.Register(autofacRoot);
            return app;
        }
    }
}
