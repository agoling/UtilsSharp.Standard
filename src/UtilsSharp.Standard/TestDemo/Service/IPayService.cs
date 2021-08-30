using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilsSharp.Dependency;

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
    }
}
