﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Jwt
{
    /// <summary>
    /// Jwt设置
    /// </summary>
    public class JwtSetting
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public string SecretKey { set; get; }
        /// <summary>
        /// 签发人
        /// </summary>
        public string Issuer { set; get; }
        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { set; get; }
        /// <summary>
        /// 过期时间||单位:s
        /// </summary>
        public int ExpireTime { set; get; }
    }
}
