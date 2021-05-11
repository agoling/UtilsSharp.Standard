using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.MVC
{
    /// <summary>
    /// Consul控制器
    /// </summary>
    public class ConsulController: BaseController
    {
        /// <summary>
        /// 健康检测
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}
