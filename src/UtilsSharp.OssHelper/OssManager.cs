using System;
using System.Collections.Generic;
using System.Text;
using OptionConfig;

namespace UtilsSharp.OssHelper
{
    /// <summary>
    /// Oss注册管理
    /// </summary>
    public class OssManager
    {
        /// <summary>
        /// 注册
        /// </summary>
        public static void Register()
        {
            var ossClient = new OssClient(OssConfig.OssSetting);
            //初始化 OssHelper
            OssHelper.Initialization(ossClient);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ossClient">OssClient</param>
        public static void Register<T>(OssClient ossClient) where T : OssHelper<T>
        {
            //初始化 OssHelper
            OssHelper<T>.Initialization(ossClient); ;
        }
    }
}
