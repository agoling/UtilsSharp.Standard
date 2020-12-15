using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 基础状态码
    /// </summary>
    public class BaseStateCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        public static int 请求成功 { get; } = 200;
        /// <summary>
        /// 业务提示
        /// </summary>
        public static int 业务提示 { get; } = 999;
        /// <summary>
        /// 接口异常
        /// </summary>
        public static int 接口异常 { get; } = 2000;
        /// <summary>
        /// 网络异常
        /// </summary>
        public static int 网络异常 { get; } = 3000;
        /// <summary>
        /// 未登录
        /// </summary>
        public static int 未登录 { get; } = 4000;
        /// <summary>
        /// 授权到期
        /// </summary>
        public static int 授权到期 { get; } = 4010;
        /// <summary>
        /// 异常错误
        /// </summary>
        public static int 异常错误 { get; } = 5000;
        /// <summary>
        /// 数据找不到
        /// </summary>
        public static int 数据找不到 { get; } = 6000;
        /// <summary>
        /// 数据验证不通过
        /// </summary>
        public static int 数据验证不通过 { get; } = 6010;
        /// <summary>
        /// 默认业务性异常
        /// </summary>
        public static int 默认业务性异常 { get; } = 7000;
        /// <summary>
        /// 数据库异常
        /// </summary>
        public static int 数据库异常 { get; } = 8000;
        /// <summary>
        /// 系统错误
        /// </summary>
        public static int 系统错误 { get; } = 9000;
    }
}
