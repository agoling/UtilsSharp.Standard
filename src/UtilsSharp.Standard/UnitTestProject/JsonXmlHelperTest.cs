using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNetCore
{

    [TestClass]
    public class JsonXmlHelperTest
    {
        [TestMethod]
        public void JsonXml()
        {
            var aa = JsonXmlHelper.XmlToJson("<Test><Name>Test class</Name><X>100</X><Y>200</Y></Test>");
            var bb= JsonXmlHelper.JsonToXml("{\"Test\":{\"Name\":\"Test class\",\"X\":\"100\",\"Y\":\"200\"}}");
          
        }
    }
}
