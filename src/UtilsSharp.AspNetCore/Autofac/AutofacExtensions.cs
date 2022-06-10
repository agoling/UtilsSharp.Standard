using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace UtilsSharp.AspNetCore.Autofac
{
    /// <summary>
    /// AutofacExtensions
    /// </summary>
    public static class AutofacExtensions
    {
        /// <summary>
        /// 注册依赖注入
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutofacExtensions(this IApplicationBuilder app)
        {
            var autofacRoot = app.ApplicationServices.GetAutofacRoot();
            AutofacContainer.Register(autofacRoot);
            return app;
        }
    }
}
