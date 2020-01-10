using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore.Core.Result
{
    /// <summary>
    /// 枚举对象模型
    /// </summary>
    public class EnumEntity
    {
        /// <summary>  
        /// 枚举名称  
        /// </summary>  
        public string EnumName { set; get; }

        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public int EnumValue { set; get; }

        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Description { set; get; }
    }
}
