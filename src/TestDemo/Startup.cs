using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestDemo.Service;
using UtilsSharp;
using UtilsSharp.AspNetCore;
using UtilsSharp.AspNetCore.Interceptor;
using UtilsSharp.AspNetCore.Swagger;
using UtilsSharp.Protobuf;

namespace TestDemo
{
    public class Startup: AutofacStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ProtobufRunTime.Initialize();

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
            
            Init<LoggerInterceptor>(builder);

            builder.RegisterType<WxPayService>().Named<IPayService>(nameof(WxPayService));
            builder.RegisterType<AliPayService>().Named<IPayService>(nameof(AliPayService));
        }
    }
}
