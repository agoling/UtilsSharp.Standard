using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Service;
using UtilsSharp.AspNetCore.MVC;

namespace TestDemo.Controllers
{
    [ApiExplorerSettings(GroupName = "test")]
    public class TestController : BaseController
    {
        private readonly IPayService _wxPayService;
        private readonly IPayService _aliPayService;
        private IComponentContext _componentContext;
        public TestController(IComponentContext componentContext)
        {
            _componentContext = componentContext;
            //解释组件
            _wxPayService = componentContext.ResolveNamed<IPayService>(typeof(WxPayService).Name);
            _aliPayService = componentContext.ResolveNamed<IPayService>(typeof(AliPayService).Name);

            //_wxPayService =AutofacContainer.Current.ResolveNamed<IPayService>(typeof(WxPayService).Name);
            //_aliPayService = AutofacContainer.Current.ResolveNamed<IPayService>(typeof(AliPayService).Name);

            //_wxPayService = AutofacContainer.Current.Resolve<IPayService>();
            //_aliPayService = AutofacContainer.Current.Resolve<IPayService>();
        }


        /// <summary>
        /// IEnumerable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            var token = UtilsSharp.Standard.HttpContext.Current.Request.Headers["token"];
            return $"{_wxPayService.PayType()}和{_aliPayService.PayType()}";
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
        /// <param name="value">value</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">value</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
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