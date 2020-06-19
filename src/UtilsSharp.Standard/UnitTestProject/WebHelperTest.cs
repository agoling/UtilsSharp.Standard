using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{

    [TestClass]
    public class WebHelperTest
    {
        [TestMethod]
        public void DoGet()
        {
            WebHelper webHelper = new WebHelper();
            var r = webHelper.DoGet(
                "https://open.kwaixiaodian.com/open/item/category?appkey=ks701435646170814247&timestamp=1592562845&access_token=ChFvYXV0aC5hY2Nlc3NUb2tlbhJQfG0MKhEksQbCRHt6qmUKtlzlrJp23_K0DiRsEZRTwpdlWhM6RBXish-I4HIYyMewNudDJ45TDkbK8S6tO1vClgLkSC5n8BVpie4Iiz7qILcaEjmLmqCDEEG6vPl6Vbu14ienTSIg4-rNQBFmd7zZrJ0YyL9EzWNkQcyMTyQOVNmSnmQFkJIoBTAB&version=1&uid=1962366901&param=&method=open.item.category");
        }
    }
}
