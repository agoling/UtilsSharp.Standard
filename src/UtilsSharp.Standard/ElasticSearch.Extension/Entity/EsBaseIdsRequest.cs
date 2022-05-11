using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilsSharp.Standard.Interface;

namespace ElasticSearch.Extension.Entity
{
    /// <summary>
    /// 基础Id集合参数
    /// </summary>
    public class EsBaseIdsRequest:IProtobufEntity
    {
        /// <summary>
        /// 素材Id
        /// </summary>
        [Display(Name = "Id集合")]
        [Required(ErrorMessage = "{0}不能为空")]
        public List<string> Ids { set; get; }
    }
}
