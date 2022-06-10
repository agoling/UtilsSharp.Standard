using Microsoft.AspNetCore.Mvc;

namespace UtilsSharp.AspNetCore.MVC
{
    /// <summary>
    /// Ip控制器
    /// </summary>
    [ApiExplorerSettings(GroupName = "api_default")]
    public class IpController : BaseController
    {
        /// <summary>
        /// 客户端
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Client()
        {
            var r = IpHelper.GetClientIp();
            return r;
        }

        /// <summary>
        /// 服务器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Server()
        {
            var r = IpHelper.GetServerIp();
            return r;
        }
    }
}
