using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class NumberHelperTest
    {
        [TestMethod]
        public void ToCnNumber()
        {
            var phoneNum = 2;
            var aa = NumberHelper.ToCnNumber(phoneNum);
        }


        [TestMethod]
        public void ToUnitMoney()
        {
            var money = 246464654654;
            var aa = NumberHelper.ToUnitMoney(money);
        }
    }
}
