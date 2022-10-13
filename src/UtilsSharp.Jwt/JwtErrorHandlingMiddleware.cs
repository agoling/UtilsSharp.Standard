using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UtilsSharp.Standard;
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace UtilsSharp.Jwt
{
    /// <summary>
    /// Jwt错误处理插件
    /// </summary>
    public class JwtErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Jwt错误处理插件
        /// </summary>
        public JwtErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// context
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var code = 200;
                var msg = "";
                switch (statusCode)
                {
                    case 401:
                        code = 4010;
                        msg = $"未授权:{statusCode}";
                        break;
                    case 403:
                        code = 4030;
                        msg = $"暂无权限:{statusCode}";
                        break;
                    case 404:
                        code = 999;
                        msg = $"未找到服务:{statusCode}";
                        break;
                    case 502:
                        code = 999;
                        msg = $"请求错误:{statusCode}";
                        break;
                    default:
                        {
                            if (statusCode != 200)
                            {
                                code = 999;
                                msg = $"未知错误:{statusCode}";
                            }
                            break;
                        }
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, code, msg);
                }
            }
        }

        /// <summary>
        /// 异常错误信息捕获，将错误信息用Json方式返回
        /// </summary>
        private static Task HandleExceptionAsync(HttpContext context, int code, string msg)
        {
            var setting = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var result = JsonConvert.SerializeObject(new BaseResult<string>() { Code = code, Msg = msg, }, setting);
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync(result);
        }
    }
}
