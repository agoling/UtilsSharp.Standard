using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.MVC
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
