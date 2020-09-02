using System;
using System.Collections.Generic;
using System.Text;
using AspNetCore.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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

        /// <summary>
        /// 注册swagger
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerExtensions(this IApplicationBuilder app)
        {
            var swaggerDocOptions = AspNetCoreExtensionsConfig.SwaggerDocOptions;
            if (!swaggerDocOptions.Enable) return app;
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{swaggerDocOptions.Name}/swagger.json", $"{swaggerDocOptions.OpenApiInfo.Title} {swaggerDocOptions.OpenApiInfo.Version}");
            });
            return app;
        }

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
