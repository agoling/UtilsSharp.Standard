using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace UtilsSharp.Jwt
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
        /// <param name="jwtSetting">Jwt参数</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtExtensions(this IServiceCollection services, JwtSetting jwtSetting)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,(options) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //验证发布者
                        ValidateIssuer = true,
                        ValidIssuer = jwtSetting.Issuer,
                        //验证接收者
                        ValidateAudience = true,
                        ValidAudience = jwtSetting.Audience,
                        //验证生命周期(是否过期)
                        ValidateLifetime = true,
                        //验证全局私钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
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
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
               .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            return services;
        }

    }
}
