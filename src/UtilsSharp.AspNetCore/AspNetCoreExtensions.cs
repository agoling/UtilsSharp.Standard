using Microsoft.Extensions.DependencyInjection;
using UtilsSharp.AspNetCore.Swagger;

namespace UtilsSharp.AspNetCore
{
    /// <summary>
    /// AspNetCoreExtensions
    /// </summary>
    public static class AspNetCoreExtensions
    {
        /// <summary>
        /// 添加AspNetCore扩展服务
        /// </summary>
        /// <param name="services">services</param>
        /// <returns></returns>
        public static IServiceCollection AddAspNetCoreExtensions(this IServiceCollection services)
        {
            services.AddSwaggerExtensions();
            return services;
        }
    }
}
