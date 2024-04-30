using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TestDemo.Service;
using UtilsSharp;
using UtilsSharp.AspNetCore;
using UtilsSharp.AspNetCore.Interceptor;
using UtilsSharp.AspNetCore.Swagger;
using UtilsSharp.Shared.Standard;
//using UtilsSharp.Grpc.AspNetCore;

namespace TestDemo
{
    public class Startup: AutofacStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddGrpcExtensions("TestDemo");
            services.AddControllers();
            AspNetCoreExtensionsConfig.SwaggerDocOptions = AppsettingsHelper.GetSection<SwaggerDocOptions>("SwaggerDocOptions");
            services.AddAspNetCoreExtensions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAspNetCoreExtensions();
            app.UseRouting();
            //注册扩展
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        /// <summary>
        /// 依赖注入映射
        /// </summary>
        /// <param name="builder">builder</param>
        public override void ConfigureContainer(ContainerBuilder builder)
        {
            ////同步拦截器
            //builder.RegisterType<WxPayService>().Named<IPayService>(nameof(WxPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(LoggerInterceptor));//注册类并为其添加拦截器  一个接口多个实现注册
            //builder.RegisterType<AliPayService>().Named<IPayService>(nameof(AliPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(LoggerInterceptor));//注册类并为其添加拦截器  一个接口多个实现注册
            ////日志拦截器
            //Init<LoggerInterceptor>(builder);

            //异步拦截器注册
            builder.RegisterType<WxPayService>().Named<IPayService>(nameof(WxPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncInterceptor<LoggerAsyncInterceptorNew>));//注册类并为其添加拦截器  一个接口多个实现注册
            builder.RegisterType<AliPayService>().Named<IPayService>(nameof(AliPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncInterceptor<LoggerAsyncInterceptorNew>));//注册类并为其添加拦截器  一个接口多个实现注册
            //日志拦截器
            Init<AsyncInterceptor<LoggerAsyncInterceptorNew>>(builder);

        }

        public class LoggerAsyncInterceptorNew: LoggerAsyncInterceptor
        {
            /// <summary>
            /// Exception 匹配规则
            /// </summary>
            public override List<ExceptionRegexRule> ExceptionRegexRule { set; get; } = new List<ExceptionRegexRule>
            {
              new ExceptionRegexRule { Code = BaseStateCode.授权过期, Rules = new List<string> { "unauthorized", "request need user authorized", "授权过期", "授权失效", "未授权" },Msg="亲，你授权已经过期了哦" },
              new ExceptionRegexRule { Code = BaseStateCode.未登录, Rules = new List<string> { "未登入", "登入过期", "登入失效", "未登录", "登录过期", "登录失效", "token过期", "token expires" },Msg="" },
              new ExceptionRegexRule { Code = BaseStateCode.数据库异常, Rules = new List<string> { "db error"},Msg="老板对不起，数据库错误了" }
            };
        }

    }
}
