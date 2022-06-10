using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDemo.Service
{
    /// <summary>
    /// 支付宝支付
    /// </summary>
    public class AliPayService : IPayService
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        /// <returns></returns>
        public string PayType()
        {
            return "支付宝支付";
        }
    }
}
