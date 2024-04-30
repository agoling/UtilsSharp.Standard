using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using TestDemo.Service;
using UtilsSharp.AspNetCore.Autofac;
using UtilsSharp.AspNetCore.MVC;
using UtilsSharp.Shared.Standard;

namespace TestDemo.Controllers
{
    [ApiExplorerSettings(GroupName = "test")]
    public class TestController : BaseController
    {
        private readonly ITestService _testService;
        private readonly IPayService _wxPayService;
        private readonly IPayService _aliPayService;
        public TestController(IComponentContext componentContext, ITestService testService)
        {
            _testService = testService;
            ////解释组件
            //_wxPayService = componentContext.ResolveNamed<IPayService>(typeof(WxPayService).Name);
            //_aliPayService = componentContext.ResolveNamed<IPayService>(typeof(AliPayService).Name);

            _wxPayService = AutofacContainer.Current.ResolveNamed<IPayService>(typeof(WxPayService).Name);
            _aliPayService = AutofacContainer.Current.ResolveNamed<IPayService>(typeof(AliPayService).Name);

        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> Pay1()
        {
            //var aa = await _testService.Pay1();
            var aa = await _wxPayService.Pay1();
            return aa;
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public string Pay2()
        {
            //var aa = _testService.Pay2();
            var aa = _wxPayService.Pay2();
            return aa;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public BaseResult<string> Pay3()
        {
            var aa = _testService.Pay3();
            //var token = _wxPayService.Pay2();
            return aa;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResult<string>> Pay4()
        {
            var aa = await _testService.Pay4();
            //var token = _wxPayService.Pay2();
            return aa;
        }

        /// <summary>
        /// IEnumerable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            var szy = UtilsSharp.HttpContext.Current.Request.Headers["szy"];
            var token = UtilsSharp.HttpContext.Current.Request.Headers["Authorization"];
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
            var token = UtilsSharp.HttpContext.Current.Request.Headers["token"];
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