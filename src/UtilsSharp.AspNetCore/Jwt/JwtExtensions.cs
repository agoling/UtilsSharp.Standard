using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace UtilsSharp.AspNetCore.Jwt
{
    /// <summary>
    /// JwtExtensions
    /// </summary>
    public static class JwtExtensions
    {

        /// <summary>
        /// 注册Jwt
        /// </summary>
        /// <param name="app">app</param>
        /// <returns></returns>
        public static IApplicationBuilder UseJwtExtensions(this IApplicationBuilder app)
        {
            //请求错误提示配置
            app.UseJwtErrorHandlingExtensions();
            //验证你是谁
            app.UseAuthentication();
            //验证你有什么权限
            app.UseAuthorization();

            return app;
        }

        /// <summary>
        /// 添加Jwt扩展
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="jwtOptions">Jwt参数</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtExtensions(this IServiceCollection services, JwtOptions jwtOptions)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //取出私钥
                    var secretByte = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //验证发布者
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        //验证接收者
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        //验证生命周期(是否过期)
                        ValidateLifetime = true,
                        //验证全局私钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretByte),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }

        /// <summary>
        /// 添加Jwt扩展
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="tokenValidationParameters">token验证参数</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtExtensions(this IServiceCollection services, TokenValidationParameters tokenValidationParameters)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            return services;
        }

    }
}
