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
            //ע����չ
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        /// <summary>
        /// ����ע��ӳ��
        /// </summary>
        /// <param name="builder">builder</param>
        public override void ConfigureContainer(ContainerBuilder builder)
        {
            ////ͬ��������
            //builder.RegisterType<WxPayService>().Named<IPayService>(nameof(WxPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(LoggerInterceptor));//ע���ಢΪ�����������  һ���ӿڶ��ʵ��ע��
            //builder.RegisterType<AliPayService>().Named<IPayService>(nameof(AliPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(LoggerInterceptor));//ע���ಢΪ�����������  һ���ӿڶ��ʵ��ע��
            ////��־������
            //Init<LoggerInterceptor>(builder);

            //�첽������ע��
            builder.RegisterType<WxPayService>().Named<IPayService>(nameof(WxPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncInterceptor<LoggerAsyncInterceptorNew>));//ע���ಢΪ�����������  һ���ӿڶ��ʵ��ע��
            builder.RegisterType<AliPayService>().Named<IPayService>(nameof(AliPayService)).EnableInterfaceInterceptors().InterceptedBy(typeof(AsyncInterceptor<LoggerAsyncInterceptorNew>));//ע���ಢΪ�����������  һ���ӿڶ��ʵ��ע��
            //��־������
            Init<AsyncInterceptor<LoggerAsyncInterceptorNew>>(builder);

        }

        public class LoggerAsyncInterceptorNew: LoggerAsyncInterceptor
        {
            /// <summary>
            /// Exception ƥ�����
            /// </summary>
            public override List<ExceptionRegexRule> ExceptionRegexRule { set; get; } = new List<ExceptionRegexRule>
            {
              new ExceptionRegexRule { Code = BaseStateCode.��Ȩ����, Rules = new List<string> { "unauthorized", "request need user authorized", "��Ȩ����", "��ȨʧЧ", "δ��Ȩ" },Msg="�ף�����Ȩ�Ѿ�������Ŷ" },
              new ExceptionRegexRule { Code = BaseStateCode.δ��¼, Rules = new List<string> { "δ����", "�������", "����ʧЧ", "δ��¼", "��¼����", "��¼ʧЧ", "token����", "token expires" },Msg="" },
              new ExceptionRegexRule { Code = BaseStateCode.���ݿ��쳣, Rules = new List<string> { "db error"},Msg="�ϰ�Բ������ݿ������" }
            };
        }

    }
}
