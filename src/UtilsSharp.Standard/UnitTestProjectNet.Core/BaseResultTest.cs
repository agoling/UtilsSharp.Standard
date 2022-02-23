using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using UtilsSharp;
using UtilsSharp.Standard;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class BaseResultTest
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="request">登入参数</param>
        /// <returns></returns>
        [TestMethod]
        public BaseResult<LoginResponse> Login(LoginRequest request)
        {
            var result = new BaseResult<LoginResponse>();
            try
            {
                if (string.IsNullOrEmpty(request.UserName))
                {
                    result.SetError("用户名不能为空", BaseStateCode.参数不能为空);
                    return result;
                }
                if (string.IsNullOrEmpty(request.UserPsw))
                {
                    result.SetError("密码不能为空", BaseStateCode.参数不能为空);
                    return result;
                }
                /*…这边省略登入业务代码…*/
                var response = new LoginResponse {UserName = request.UserName, Token = Guid.NewGuid().ToString("N")};
                result.SetOkResult(response,"登入成功");
                return result;
            }
            catch (Exception ex)
            {
                var errorCode = Guid.NewGuid().ToString("N");
                //这边用errorCode作为日志主键，把错误信息写入到日志
                var errorMessage = errorCode.ToMsgException();
                result.SetError(errorMessage, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }


        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="request">登入参数</param>
        /// <returns></returns>
        [TestMethod]
        public BasePagedResult<LoginResponse> Search(BaseSortPage request)
        {
            var result = new BasePagedResult<LoginResponse>();
            try
            {
                var response = new BasePagedInfoResult<LoginResponse>()
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
                if (string.IsNullOrEmpty(request.Keyword))
                {
                    result.SetError("搜索关键词不能为空！",6010);
                    return result;
                }
                /*…这边省略查寻业务代码…*/
                /*…以下模拟从数据库获取数据…*/
                response.List = new List<LoginResponse>
                {
                    new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx1"},
                    new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx2"},
                    new LoginResponse() {Token = Guid.NewGuid().ToString(), UserName = "xxx3"}
                };
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize;
                response.TotalCount = 100;
                response.Params =JsonConvert.SerializeObject(request);
                result.SetOkResult(response);
                return result;
            }
            catch (Exception ex)
            {
                var errorCode = Guid.NewGuid().ToString("N");
                //这边用errorCode作为日志主键，把错误信息写入到日志
                var errorMessage = errorCode.ToMsgException();
                result.SetError(errorMessage, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }
    }

    /// <summary>
    /// 会员登录请求参数
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPsw { set; get; }

    }

    /// <summary>
    /// 会员登录返回参数
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// 访问口令
        /// </summary>
        public string Token { set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
    }
}
