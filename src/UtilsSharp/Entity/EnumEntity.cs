using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp.Standard.Interface;

namespace UtilsSharp.Entity
{
    /// <summary>
    /// 枚举对象模型
    /// </summary>
    public class EnumEntity: IProtobufEntity
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
