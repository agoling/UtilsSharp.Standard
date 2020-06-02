using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using UtilsSharp.Entity;
using UtilsSharp.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// 公共工具类
    /// </summary>
    public static class UtilsHelper
    {
        /// <summary>  
        /// 提取开启代理/cdn服务后的客户端真实IP  
        /// </summary>  
        /// <returns></returns>  
        public static string GetIp()
        {
            string ip;
            string xForwardedFor = HttpContext.Current.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrWhiteSpace(xForwardedFor))
            {
                ip = xForwardedFor;
            }
            else
            {
                string cfConnectingIp = HttpContext.Current.Request.Headers["CF-Connecting-IP"];
                ip = !string.IsNullOrWhiteSpace(cfConnectingIp) ? cfConnectingIp : HttpContext.Current.Request.Host.Value;
            }
            return ip;
        }

        /// <summary>
        /// 计算进度条进度
        /// </summary>
        /// <param name="step">增加的量</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="completeProgress">总完成进度</param>
        /// <returns></returns>
        public static int CalcProgress(int step, int totalCount, int completeProgress = 100)
        {
            try
            {
                var curCount = step;
                var progress = (int)(curCount * 1.0 / totalCount * completeProgress);
                if (step == totalCount)
                {
                    progress = completeProgress;
                }
                return progress;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 反射对象深度拷贝
        /// </summary>
        /// <typeparam name="T">对象模型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            try
            {
                //如果是字符串或值类型则直接返回
                if (obj is string || obj.GetType().IsValueType) return obj;
                var oldObjJson = JsonConvert.SerializeObject(obj);//序列化
                var newObj = JsonConvert.DeserializeObject<T>(oldObjJson);
                return newObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
