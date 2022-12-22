using System;
using UtilsSharp.Shared.Interface;

namespace UtilsSharp.ElasticSearch.Extension.Entity
{
    /// <summary>
    /// 基础保存参数
    /// </summary>
    public class EsBaseSaveRequest: IProtobufEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { set; get; }
    }
}
