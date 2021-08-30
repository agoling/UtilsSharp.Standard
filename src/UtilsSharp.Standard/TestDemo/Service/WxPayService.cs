using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDemo.Service
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WxPayService : IPayService
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        /// <returns></returns>
        public string PayType()
        {
            return "微信支付";
        }
    }
}
