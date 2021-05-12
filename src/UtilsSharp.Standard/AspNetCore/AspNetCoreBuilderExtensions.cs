using AspNetCore.Autofac;
using AspNetCore.Swagger;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace AspNetCore
{
    /// <summary>
    /// AspNetCoreBuilderExtensions
    /// </summary>
    public static class AspNetCoreBuilderExtensions
    {
        /// <summary>
        /// 注册AspNetCore扩展
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAspNetCoreExtensions(this IApplicationBuilder app)
        {
            //注册依赖注入
            app.UseAutofacExtensions();
            //注册HttpContext
            var httpContextAccessor = AutofacContainer.Current.Resolve<IHttpContextAccessor>();
            UtilsSharp.Standard.HttpContext.Register(httpContextAccessor);
            //注册swagger
            app.UseSwaggerExtensions();
            return app;
        }
    }
}
