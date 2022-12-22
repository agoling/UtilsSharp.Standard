using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UtilsSharp.ElasticSearch.Extension
{
    /// <summary>
    /// 验证入参模型帮助类
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// 验证模型参数
        /// </summary>
        /// <param name="value">模型对象</param>
        /// <returns></returns>
        public static ValidResult IsValid(object value)
        {
            var result = new ValidResult();
            try
            {
                var validationContext = new ValidationContext(value, null, null);
                var results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(value, validationContext, results, true);

                if (!isValid)
                {
                    result.IsValid = false;
                    result.ErrorMembers = new List<ErrorMember>();
                    foreach (var item in results)
                    {
                        result.ErrorMembers.Add(new ErrorMember()
                        {
                            ErrorMessage = item.ErrorMessage,
                            ErrorMemberName = item.MemberNames.FirstOrDefault()
                        });
                    }
                }
                else
                {
                    result.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMembers = new List<ErrorMember>
                {
                    new ErrorMember()
                    {
                        ErrorMessage = ex.Message,
                        ErrorMemberName = "Internal Error"
                    }
                };
            }
            return result;
        }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidResult
    {
        /// <summary>
        /// 错误成员集合
        /// </summary>
        public List<ErrorMember> ErrorMembers { get; set; }
        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValid { get; set; }
    }

    /// <summary>
    /// 错误成员
    /// </summary>
    public class ErrorMember
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 错误成员名称
        /// </summary>
        public string ErrorMemberName { get; set; }
    }
}
