using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 基础状态码
    /// </summary>
    public abstract class BaseStateCode
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
        /// 接口异常[2000~2999接口类异常]
        /// </summary>
        public static int 接口异常 { get; } = 2000;
        /// <summary>
        /// 网络异常[3000~3999网络类异常]
        /// </summary>
        public static int 网络异常 { get; } = 3000;
        /// <summary>
        /// 未登录[4000~4999登入授权类异常]
        /// </summary>
        public static int 未登录 { get; } = 4000;
        /// <summary>
        /// 授权过期[4000~4999登入授权类异常]
        /// </summary>
        public static int 授权过期 { get; } = 4010;
        /// <summary>
        /// TryCatch异常错误[5000~5999TryCatch类异常]
        /// </summary>
        public static int TryCatch异常错误 { get; } = 5000;
        /// <summary>
        /// 数据找不到[6000~6999数据类异常]
        /// </summary>
        public static int 数据找不到 { get; } = 6000;
        /// <summary>
        /// 数据验证不通过[6000~6999数据类异常]
        /// </summary>
        public static int 数据验证不通过 { get; } = 6010;
        /// <summary>
        /// 默认业务性错误[7000~7999其他业务及参数类异常]
        /// </summary>
        public static int 默认业务性错误 { get; } = 7000;
        /// <summary>
        /// 参数不能为空[7000~7999其他业务及参数类异常]
        /// </summary>
        public static int 参数不能为空 { get; } = 7010;
        /// <summary>
        /// 非法参数[7000~7999其他业务及参数类异常]
        /// </summary>
        public static int 非法参数 { get; } = 7020;
        /// <summary>
        /// 数据库异常[8000~8999数据库类异常]
        /// </summary>
        public static int 数据库异常 { get; } = 8000;
        /// <summary>
        /// 系统错误
        /// </summary>
        public static int 系统错误 { get; } = 9000;
    }
}
