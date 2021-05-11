using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UtilsSharp;

namespace AspNetCore.Swagger
{
    /// <summary>
    /// SwaggerExtensions
    /// </summary>
    public static class SwaggerExtensions
    {

        /// <summary>
        /// 注册swagger
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerExtensions(this IApplicationBuilder app)
        {
            if (AspNetCoreExtensionsConfig.SwaggerDocOptions == null)
            {
                return app;
            }
            try
            {
                if (!AspNetCoreExtensionsConfig.SwaggerDocOptions.Enable) return app;
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/{AspNetCoreExtensionsConfig.SwaggerDocOptions.Name}/swagger.json", $"{AspNetCoreExtensionsConfig.SwaggerDocOptions.OpenApiInfo.Title} {AspNetCoreExtensionsConfig.SwaggerDocOptions.OpenApiInfo.Version}");
                });
                return app;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UseSwaggerExtensions Exception:{ex.Message}");
                return app;
            }
        }


        /// <summary>
        /// 添加Swagger扩展服务
        /// </summary>
        /// <param name="services">services</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services)
        {
            if (AspNetCoreExtensionsConfig.SwaggerDocOptions == null)
            {
                return services;
            }
            try
            {
                if (AspNetCoreExtensionsConfig.SwaggerDocOptions.Enable)
                {
                    services.AddSwaggerGen(c =>
                    {
                        c.CustomSchemaIds(i => i.FullName);
                        c.SwaggerDoc(AspNetCoreExtensionsConfig.SwaggerDocOptions.Name, AspNetCoreExtensionsConfig.SwaggerDocOptions.OpenApiInfo);
                        var enumerable = AssemblyHelper.GetAllAssemblies();
                        foreach (var item in enumerable)
                        {
                            var xmlName = $"{item.GetName().Name}.xml";
                            var xmlPath = item.ManifestModule.FullyQualifiedName.Replace(item.ManifestModule.Name, xmlName);
                            if (File.Exists(xmlPath))
                            {
                                c.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）
                            }
                        }
                        if (AspNetCoreExtensionsConfig.SwaggerDocOptions.HeaderParameters != null && AspNetCoreExtensionsConfig.SwaggerDocOptions.HeaderParameters.Count > 0)
                        {
                            c.OperationFilter<AddRequiredHeaderParameter>();//添加header参数
                        }
                    });
                }
                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddSwaggerExtensions Exception:{ex.Message}");
                return services;
            }
        }
    }
}
