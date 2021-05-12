using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDemo.Areas.Admin.Models
{
    /// <summary>
    /// 测试参数
    /// </summary>
    public class TestRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { set; get; }
    }
}
