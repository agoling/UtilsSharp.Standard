using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using UtilsSharp.Logger;
using UtilsSharp.Standard;

namespace UtilsSharp.AspNetCore.Filter
{
    /// <summary>
    /// 参数验签
    /// </summary>
    public class RequestFilterAttribute : Attribute, IAuthorizationFilter
    {
        private int _expireTime;

        /// <summary>
        /// 参数验签
        /// </summary>
        public RequestFilterAttribute()
        {

        }
        /// <summary>
        /// 参数验签
        /// </summary>
        /// <param name="expireTime">签名过期时间|单位秒，默认2分钟</param>
        public RequestFilterAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="context">context</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Method.ToLower().Equals("post")) return;
            using var ms = new MemoryStream();
            context.HttpContext.Request.EnableBuffering();
            context.HttpContext.Request.Body.CopyToAsync(ms);
            var body = Encoding.UTF8.GetString(ms.ToArray());
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            if (_expireTime == default)
            {
                _expireTime = 2 * 60;
            }
            var result = VerifySign(context.HttpContext.Request.Headers, body, _expireTime);
            if (result.Code != 200)
            {
                context.Result = new JsonResult(result);
            }
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="headers">headers</param>
        /// <param name="body">body参数</param>
        /// <param name="expireTime">缓存过期时间</param>
        /// <returns></returns>
        private static BaseResult<string> VerifySign(IHeaderDictionary headers, string body, int expireTime)
        {
            var result = new BaseResult<string>();
            //rfa-r 随机码 rfa-t 时间戳 rfa-s 参数md5后
            string r = string.Empty, t = string.Empty, s = string.Empty;
            var bodyMd5 = body.ToMd5();
            var sign = string.Empty;
            try
            {
                r = headers["rfa-r"];//前端随机码
                t = headers["rfa-t"];//前端时间戳
                s = headers["rfa-s"];//前端签名

                if (string.IsNullOrWhiteSpace(r) || string.IsNullOrWhiteSpace(t) || string.IsNullOrWhiteSpace(s))
                {
                    result.SetError("请求签名不能为空", 6010);
                    return result;
                }

                #region 验证签名是否失效
                var cacheHelper = new CacheHelper();
                var cacheKey = $"rfa-key:{s}";
                if (cacheHelper.IsExists(cacheKey))
                {
                    result.SetError("签名已失效", 6010);
                    return result;
                }

                #endregion

                #region 验证签名是否非法
                sign = $"{r}|{bodyMd5}*{t}".ToMd5();//后端签名

                if (sign != s)
                {
                    result.SetError("非法签名", 500);
                    return result;
                }
                #endregion

                #region 验证签名是否过期
                var valTime = TimeHelper.TimeStampToDateTime(t);
                var curTime = DateTime.UtcNow.AddHours(8);

                var timeInterval = TimeHelper.GetTimeSpan(curTime, valTime).TotalSeconds;
                if (valTime > curTime)
                {
                    result.SetError("非法签名时间戳", 6010);
                    return result;
                }

                if (timeInterval > expireTime)
                {
                    result.SetError("签名已过期", 6010);
                    return result;
                }
                #endregion

                cacheHelper.Set(cacheKey, "1", expireTime);
            }
            catch (Exception ex)
            {
                result.SetError("验签失败!", 5000);
                LogHelper.Error("验签失败异常", ex, parameters: JsonConvert.SerializeObject(new { body, r, t, s, sign }));
            }
            return result;
        }

    }
}
