using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace UtilsSharp.AspNetCore.Swagger
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
                //启用中间件服务生成Swagger作为JSON终结点
                app.UseSwagger();
                //启用中间件服务对swagger-ui，指定Swagger JSON终结点
                app.UseSwaggerUI(suoption =>
                {
                    var g = new SwaggerGroup
                    {
                        GroupName = "api_default", Title = "My API", Description = "接口默认文档", Version = "v1.0"
                    };
                    if (!string.IsNullOrEmpty(AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectName))
                    {
                        g.Title = AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectName;
                    }
                    if (!string.IsNullOrEmpty(AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectDescription))
                    {
                        g.Description = AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectDescription;
                    }
                    if (!string.IsNullOrEmpty(AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectVersion))
                    {
                        g.Version = AspNetCoreExtensionsConfig.SwaggerDocOptions.ProjectVersion;
                    }
                    var groups = new List<SwaggerGroup> {g};
                    if (AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups!= null&& AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups.Count>0)
                    {
                        AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups.ForEach((item) =>
                        {
                            if(string.IsNullOrEmpty(item.GroupName)) return;
                            if(groups.Any(t=>t.GroupName==item.GroupName)) return;
                            groups.Add(item);
                        });
                    }
                    AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups=new List<SwaggerGroup>();
                    AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups.AddRange(groups);
                    suoption.RoutePrefix = string.Empty;
                    AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups.ForEach(group =>
                    {
                        suoption.DefaultModelExpandDepth(2);
                        suoption.DefaultModelRendering(ModelRendering.Example);
                        suoption.DefaultModelsExpandDepth(-1);

                        suoption.DisplayRequestDuration();
                        suoption.DocExpansion(DocExpansion.None);
                        suoption.EnableDeepLinking();
                        suoption.EnableFilter();
                        suoption.MaxDisplayedTags(int.MaxValue);
                        suoption.ShowExtensions();
                        suoption.EnableValidator();
                        suoption.SwaggerEndpoint($"/swagger/{group.GroupName}/swagger.json", $"{group.Title} {group.Version}");
                        suoption.RoutePrefix = string.Empty;

                    });
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
                        #region Swagger接口模块分组配置
                        AspNetCoreExtensionsConfig.SwaggerDocOptions.Groups.ForEach(group =>
                        {
                            c.SwaggerDoc(group.GroupName, new OpenApiInfo { Title = group.Title, Description = group.Description, Version = group.Version });
                        });
                        #endregion
                        var enumerable = AssemblyHelper.GetAllAssemblies();
                        foreach (var xmlPath in from item in enumerable let xmlName = $"{item.GetName().Name}.xml" select item.ManifestModule.FullyQualifiedName.Replace(item.ManifestModule.Name, xmlName) into xmlPath where File.Exists(xmlPath) select xmlPath)
                        {
                            c.IncludeXmlComments(xmlPath, true); //添加控制器层注释（true表示显示控制器注释）
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
