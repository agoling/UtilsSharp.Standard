using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    /// <summary>
    /// 服务器缓存帮助类
    /// </summary>
    [TestClass]
    public class CacheHelperTest
    {

        [TestMethod]
        public void GetCacheTypeName()
        {
            CacheHelper cacheHelper = new CacheHelper();
            var cacheTypeName = cacheHelper.GetCacheTypeName();
        }

        [TestMethod]
        public void Set()
        {
            CacheHelper cacheHelper = new CacheHelper();
            cacheHelper.Set("abc", "123456", 30);

            var data= cacheHelper.Get<string>("abc");
        }


    }
}
