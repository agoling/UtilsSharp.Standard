using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace UtilsSharp
{
    /// <summary>
    /// JsonXml帮助类
    /// </summary>
    public class JsonXmlHelper
    {
        /// <summary>
        /// json转xml
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static string JsonToXml(string json)
        {
            var doc = JsonConvert.DeserializeXmlNode(json);
            var xml = doc.InnerXml;
            return xml;
        }

        /// <summary>
        /// xml转json
        /// </summary>
        /// <param name="xml">xml字符串</param>
        /// <returns></returns>
        public static string XmlToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }

    }
}
