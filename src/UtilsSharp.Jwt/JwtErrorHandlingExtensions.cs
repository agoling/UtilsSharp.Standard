using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace UtilsSharp.Jwt
{
    /// <summary>
    /// Jwt错误处理扩展方法
    /// </summary>
    public static class JwtErrorHandlingExtensions
    {
        /// <summary>
        /// Jwt错误处理扩展方法
        /// </summary>
        public static IApplicationBuilder UseJwtErrorHandlingExtensions(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JwtErrorHandlingMiddleware>();
        }
    }
}
