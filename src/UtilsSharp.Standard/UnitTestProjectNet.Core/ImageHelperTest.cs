using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilsSharp;

namespace UnitTestProjectNet.Core
{
    [TestClass]
    public class ImageHelperTest
    {
        [TestMethod]
        public void CreateImageFromBytes()
        {
          var image= Image.FromFile("D:\\代表性\\11.jpg");
          var bytes = ImageHelper.ImageToBytes(image);
          var aa = ImageHelper.CreateImageFromBytes("D:\\代表性\\", bytes);

        }

    }
}
