using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;
using UtilsSharp.Standard;

namespace UnitTestProjectNetCore
{
    [TestClass]
    public class BaseResultTest
    {
        [TestMethod]
        public void BaseResultTestDemo()
        {
           BaseResult<UserInfo> result0 = new BaseResult<UserInfo>();
           result0.SetOkResult(null,"执行成功");

            BaseResult<UserInfo> result1=new BaseResult<UserInfo>();
           var userInfo1 = new UserInfo {UserName = "123", UserPsw = "123"};
           result1.SetOkResult(userInfo1);

           BaseResult<UserInfo> result2 = new BaseResult<UserInfo>();
           result2.Result = new UserInfo { UserName = "456", UserPsw = "456" };
           result2.SetOkResult(null,"执行成功");

           BaseResult<UserInfo> result20 = new BaseResult<UserInfo>();
           result20.Result = new UserInfo { UserName = "4567", UserPsw = "4567" };
           result20.SetOkResult(result20.Result, "执行成功");

           BaseResult<string> result3 = new BaseResult<string>();
           result3.SetOkResult("", "执行成功");

           BaseResult<string> result4 = new BaseResult<string>();
           result4.SetOkResult("执行成功");

           BaseResult<string> result5 = new BaseResult<string>();
           result5.SetOk("执行成功");
        }

        [TestMethod]
        public void BasePagedResultTestDemo()
        {
            BasePagedResult<UserInfo> result0=new BasePagedResult<UserInfo>();
            result0.SetOk("请求成功");

            BasePagedResult<UserInfo> result1 = new BasePagedResult<UserInfo>();
            result1.SetOkResult(new BasePagedInfoResult<UserInfo>(),"请求成功");

        }
    }

    public class UserInfo
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
}
