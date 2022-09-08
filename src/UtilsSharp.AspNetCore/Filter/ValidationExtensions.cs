using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace UtilsSharp.AspNetCore.Filter
{
    /// <summary>
    ///  验证入参模型插件
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// 添加入参验证扩展服务
        /// </summary>
        /// <param name="services">services</param>
        /// <returns></returns>
        public static IServiceCollection AddValidationExtensions(this IServiceCollection services)
        {
            //关闭参数自动校验,我们需要返回自定义的格式
            services.Configure<ApiBehaviorOptions>((o) =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });
            //添加参数过滤
            services.AddControllers(o =>
            {
                o.Filters.Add<ValidationActionFilter>();
            });
            return services;
        }
    }
}
