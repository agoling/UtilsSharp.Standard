using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.AspNetCore.Filter
{
    /// <summary>
    /// 验证入参模型过滤器
    /// </summary>
    public class ValidationActionFilter : IActionFilter
    {

        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //校验参数
            if (context.ModelState.IsValid) return;
            var errorMsg = context.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
            context.Result = new OkObjectResult(new
            {
                    Code = BaseStateCode.非法参数,
                    Msg = string.IsNullOrWhiteSpace(errorMsg) ? "参数校验错误" : errorMsg,
                    Result=default(object)
                });
            return;
        }

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
