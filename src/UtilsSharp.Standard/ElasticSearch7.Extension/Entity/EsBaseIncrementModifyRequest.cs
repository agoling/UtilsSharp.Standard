using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UtilsSharp.Standard.Interface;

namespace ElasticSearch7.Extension.Entity
{
    /// <summary>
    /// 基础增量修改参数
    /// </summary>
    public class EsBaseIncrementModifyRequest: IProtobufEntity
    {
        /// <summary>
        /// Id集合
        /// </summary>
        [Display(Name = "Id集合")]
        [Required(ErrorMessage = "{0}不能为空")]
        public List<string> Ids { set; get; }

        /// <summary>
        /// 增量修改数据
        /// </summary>
        [Display(Name = "增量修改数据")]
        [Required(ErrorMessage = "{0}不能为空")]
        public Dictionary<string, object> Data { set; get; }
    }
}
