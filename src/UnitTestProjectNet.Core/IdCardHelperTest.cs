using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class IdCardHelperTest
    {
        [TestMethod]
        public void GetCharLengthTest()
        {
            var num = "";
            var bb = IdCardHelper.IsIdCard(num);
           
        }

        [TestMethod]
        public void GetIdCardAddress()
        {
            var num = "";
            var bb = IdCardHelper.GetIdCardAddress(num);

        }


        [TestMethod]
        public void GetIdCardInfo()
        {
            var num = "";
            var bb = IdCardHelper.GetIdCardInfo(num);

        }


        





    }
}
