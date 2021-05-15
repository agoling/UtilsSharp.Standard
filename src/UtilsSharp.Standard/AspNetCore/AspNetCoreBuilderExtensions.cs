using AspNetCore.Autofac;
using AspNetCore.Swagger;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using UtilsSharp;

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
            //注册后使可以支持.Net平台上不支持的编码
            StringHelper.EncodingRegister();
            return app;
        }
    }
}
