using System.ComponentModel.DataAnnotations;
using UtilsSharp.Shared.Interface;

namespace UtilsSharp.ElasticSearch7.Extension.Entity
{
    /// <summary>
    /// 基础单条Id参数
    /// </summary>
    public class EsBaseIdRequest: IProtobufEntity
    {
        /// <summary>
        /// 素材Id
        /// </summary>
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string Id { set; get; }
    }
}
