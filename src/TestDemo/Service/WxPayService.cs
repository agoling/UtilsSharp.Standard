using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsSharp.Logger;
using UtilsSharp.Shared.Standard;

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

        private readonly ITest2 _test2;

        public WxPayService(ITest2 test2)
        {
            _test2 = test2;
        }

        public async Task<string> Pay1()
        {

            //var r = await PayMethod1();
            var r = await _test2.PayMethod1();
            return r;

        }

        public async Task<int> PayMethod1()
        {
            int i = 1;
            int j = 0;

            await Task.Run(() => { });

            return i / j;
        }


        public string Pay2()
        {
            //var r = PayMethod2();
            //var r = _test2.PayMethod2();
            //return r;
            int i = 1;
            int j = 0;
            return (i / j).ToString();
            
        }

        public string PayMethod2()
        {
            int i = 1;
            int j = 0;
            return (i / j).ToString();
        }

        public BaseResult<string> Pay3()
        {
            //var r = PayMethod2();
            var r = _test2.PayMethod3();
            return r;
        }

        public BaseResult<string> PayMethod3()
        {
            BaseResult<string> result = new BaseResult<string>();
            int i = 1;
            int j = 0;
            result.Result = (i / j).ToString();
            return result;
        }

    }
}
