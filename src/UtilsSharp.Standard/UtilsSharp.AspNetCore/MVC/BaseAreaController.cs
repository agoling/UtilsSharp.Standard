using Microsoft.AspNetCore.Mvc;

namespace UtilsSharp.AspNetCore.MVC
{
    /// <summary>
    /// 基础区域控制器
    /// </summary>
    [Route("api/[area]/[controller]/[action]")]
    [ApiController]
    public class BaseAreaController : Controller
    {

    }
}
