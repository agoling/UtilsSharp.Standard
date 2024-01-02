using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsSharp.Shared.Dependency;
using UtilsSharp.Shared.Standard;

namespace TestDemo.Service
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public interface IPayService:IUnitOfWorkDependency
    {
        /// <summary>
        /// 支付类型
        /// </summary>
        /// <returns></returns>
        string PayType();

        Task<string> Pay1();
        string Pay2();
        BaseResult<string> Pay3();
    }
}
