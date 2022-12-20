using System.Configuration;
using System.Xml;

namespace UtilsSharp
{
    /// <summary>
    /// 通过Section生成配置文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfigurationSectionHandler<T>:IConfigurationSectionHandler where T : class, new()
    {
        /// <summary>
        /// 生成配置文件
        /// </summary>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new T();
            if (section == null) return config;
            var properties = typeof(T).GetProperties();
            foreach (XmlNode cNode in section.ChildNodes)
            {
                foreach (var item in properties)
                {
                    if (cNode.Name.ToUpper() != item.Name.ToUpper()) continue;
                    item.SetValue(config, cNode.InnerText, null);
                    break;
                }
            }
            return config;
        }
    }
}
