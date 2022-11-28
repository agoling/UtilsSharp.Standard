using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UtilsSharp
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(this string json)
        {
            if (string.IsNullOrEmpty(json)) return json;
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

        /// <summary>
        /// 把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="obj">Json 对象</param>
        /// <param name="camelCase">是否小写名称</param>
        /// <param name="indented"></param>
        /// <returns>Json 字符串</returns>
        public static string ToJson(this object obj, bool camelCase = false, bool indented = false)
        {
            if (obj == null) return "";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (camelCase)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented)
            {
                settings.Formatting = Formatting.Indented;
            }
            string json = JsonConvert.SerializeObject(obj, settings);
            return JsonDateTimeFormat(json);
        }

        /// <summary>
        /// 把Json字符串转换为强类型对象
        /// </summary>
        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            json = JsonDateTimeFormat(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// 反射对象深度拷贝
        /// </summary>
        /// <typeparam name="T">对象模型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            try
            {
                //如果是字符串或值类型则直接返回
                if (obj is string || obj.GetType().IsValueType) return obj;
                var oldObjJson = JsonConvert.SerializeObject(obj);//序列化
                var newObj = JsonConvert.DeserializeObject<T>(oldObjJson);
                return newObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
