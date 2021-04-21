using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UtilsSharp
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    public class DictionaryHelper
    {

        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> ObjToDictionaryStringValue(object obj)
        {
            var objDic = ObjToDictionary(obj);
            var strDic = new Dictionary<string, string>();
            if (objDic == null) return strDic;
            foreach (var item in objDic)
            {
                if (string.IsNullOrEmpty(item.Key) || item.Value == null) continue;
                var key = item.Key;
                object objValue;
                try
                {
                    objValue = Convert.ChangeType(item.Value, typeof(string));
                }
                catch (Exception)
                {
                    objValue = JsonConvert.SerializeObject(item.Value);
                }
                var value = objValue.ToString();
                strDic.Add(key, value);
            }
            return strDic;
        }

        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, object> ObjToDictionary(object obj)
        {
            return ObjToDictionary(obj, null, null);
        }

        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="members">需要转换的成员</param>
        /// <param name="ignoreMembers">忽略转换的成员</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, object> ObjToDictionary(object obj, string[] members, string[] ignoreMembers)
        {
            if (obj == null) return null;
            // 创建目标字典
            var dictionary = new Dictionary<string, object>();
            // 获取对象类型
            var type = obj.GetType();
            // 获取成员字段
            var fieldInfos = type.GetFields();
            // 遍历成员字段，添加要转换的成员字段到字典中
            foreach (var fieldInfo in fieldInfos)
            {
                // 获取当前字段名
                var name = fieldInfo.Name;
                // 判断当前字段是否需要转换
                if ((members != null && members.Length > 0 && !members.Contains("*") && !members.Contains(name)) ||
                    (ignoreMembers != null && ignoreMembers.Length > 0 &&
                     (ignoreMembers.Contains("*") || ignoreMembers.Contains(name)))) continue;
                // 判断当前字段是否是公开的
                if (fieldInfo.IsPublic)
                {
                    // 判断当前字段是否是静态的
                    if (fieldInfo.IsStatic)
                    {
                        // 获取成员字段的静态值并设置到字典中
                        dictionary[name] = fieldInfo.GetValue(null);
                    }
                    else
                    {
                        // 获取成员字段的实例值并设置到字典中
                        dictionary[name] = fieldInfo.GetValue(obj);
                    }
                }
            }
            // 获取成员属性
            var propertyInfos = type.GetProperties();
            // 遍历成员属性，添加要转换的成员属性到字典中
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                // 获取当前属性名
                string name = propertyInfo.Name;
                // 判断当前属性是否需要转换
                if ((members != null && members.Length > 0 && !members.Contains("*") && !members.Contains(name)) ||
                    (ignoreMembers != null && ignoreMembers.Length > 0 &&
                     (ignoreMembers.Contains("*") || ignoreMembers.Contains(name)))) continue;
                // 判断当前属性是否可读的并且是简单的类型
                if (!propertyInfo.CanRead) continue;
                // 获取当前属性的GET访问器
                var methodInfo = propertyInfo.GetGetMethod();
                // 判断当前属性的GET访问器是否是公开的
                if (!methodInfo.IsPublic) continue;
                // 判断当前属性的GET访问器是否是静态的
                if (methodInfo.IsStatic)
                {
                    // 获取成员属性的静态值并设置到字典中
                    dictionary[name] = propertyInfo.GetValue(null, null);
                }
                else
                {
                    // 获取成员属性的实例值并设置到字典中
                    dictionary[name] = propertyInfo.GetValue(obj, null);
                }
            }
            // 返回字典
            return dictionary;
        }

        /// <summary>
        /// 转换字典为对象
        /// </summary>
        /// <param name="dictionary">要转换的字典</param>
        /// <returns>返回对象</returns>
        public static T DictionaryToObj<T>(Dictionary<string, object> dictionary) where T : class, new()
        {
            return DictionaryToObj<T>(dictionary, null,null);
        }

        /// <summary>
        /// 转换字典为对象
        /// </summary>
        /// <param name="dictionary">要转换的字典</param>
        /// <param name="members">需要转换的成员</param>
        /// <param name="ignoreMembers">忽略转换的成员</param>
        /// <returns>返回对象</returns>
        public static T DictionaryToObj<T>(Dictionary<string, object> dictionary, string[] members, string[] ignoreMembers)where T:class,new()
        {
            if (dictionary == null) return null;
            //创建对象获取类型
            var type = typeof(T);
            // 创建目标对象
            var obj = Activator.CreateInstance(type);
            // 遍历字典，添加要转换的数据到对象成员中
            foreach (KeyValuePair<string, object> item in dictionary)
            {
                // 获取成员名称
                var name = item.Key;
                // 判断当前成员是否需要转换
                if ((members != null && members.Length > 0 && !members.Contains("*") && !members.Contains(name)) ||
                    (ignoreMembers != null && ignoreMembers.Length > 0 &&
                     (ignoreMembers.Contains("*") || ignoreMembers.Contains(name)))) continue;
                // 获取当前成员名称对应的数据
                var value = item.Value;
                // 根据当前成员名称获取成员属性
                var propertyInfo = type.GetProperty(name);
                // 判断是否有对应名称的成员属性
                if (propertyInfo == null)
                {
                    // 根据当前列名获取成员字段
                    var fieldInfo = type.GetField(name);
                    // 判断成员字段是否存在并且是否是公开可写的
                    if (fieldInfo == null || !fieldInfo.IsPublic || fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                        continue;
                    // 根据成员字段类型转换数据类型
                    value = fieldInfo.FieldType.BaseType == typeof(Enum) ? Enum.Parse(fieldInfo.FieldType, value.ToString()) : Convert.ChangeType(value, fieldInfo.FieldType); 
                    // 判断数据转换是否成功
                    if (value == null) continue;
                    // 判断成员字段是否是静态的
                    fieldInfo.SetValue(fieldInfo.IsStatic ? null : obj, value);
                }
                else
                {
                    // 判断成员属性是否可写
                    if (!propertyInfo.CanWrite) continue;
                    // 获取成员属性的SET访问器
                    var methodInfo = propertyInfo.GetSetMethod();
                    // 判断成员属性的SET访问器是否是公开的
                    if (!methodInfo.IsPublic) continue;
                    // 根据成员属性类型转换数据类型
                    value = propertyInfo.PropertyType.BaseType == typeof(Enum) ? Enum.Parse(propertyInfo.PropertyType, value.ToString()) : Convert.ChangeType(value, propertyInfo.PropertyType);
                    // 判断数据转换是否成功
                    if (value == null) continue;
                    // 判断成员属性的SET访问器是否是静态的
                    propertyInfo.SetValue(methodInfo.IsStatic ? null : obj, value, null);
                }
            }
            // 返回目标对象
            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
