using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MVC;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Areas.Admin.Models;

namespace TestDemo.Areas.Admin.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Area("admin")]
    public class TestController : BaseAreaController
    {
        /// <summary>
        /// IEnumerable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            var token = UtilsSharp.Standard.HttpContext.Current.Request.Headers["token"];
            return "value";
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var token = UtilsSharp.Standard.HttpContext.Current.Request.Headers["token"];
            return "value";
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="TestRequest">request</param>
        [HttpPost]
        public string Post(TestRequest request)
        {
            return request.UserName + request.Password;
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">value</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
