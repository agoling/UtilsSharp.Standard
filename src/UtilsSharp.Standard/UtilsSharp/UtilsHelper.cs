using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
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
        /// 获取客户端Ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            var ip = HttpContext.Current.Connection.RemoteIpAddress.ToString();
            return ip;
        }

        /// <summary>
        /// 获取服务端Ip
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList
                .FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString();
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
