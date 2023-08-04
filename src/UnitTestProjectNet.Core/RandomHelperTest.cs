using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class RandomHelperTest
    {


        [TestMethod]
        public void aa()
        {

            //调用函数产生4个随机中文汉字编码
            var aa= RandomHelper.SimpleChineseCharacters();

            var bb = RandomHelper.ChineseCharacters();

            

        }
    }

}
