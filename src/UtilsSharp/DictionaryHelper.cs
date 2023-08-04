﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace UtilsSharp
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    public static class DictionaryHelper
    {

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dic">字典</param>
        /// <param name="key">字典Key</param>
        /// <returns></returns>
        public static string GetValue(this Dictionary<string, string> dic, string key)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(key))
            {
                return result;
            }
            if (dic.ContainsKey(key))
            {
                result = dic[key];
            }
            return result;
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dic">字典</param>
        /// <param name="key">字典Key</param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, object> dic, string key)
        {
            var result = default(T);
            if (string.IsNullOrEmpty(key))
            {
                return result;
            }
            if (dic.ContainsKey(key))
            {
                result = (T)dic[key];
            }
            return result;
        }

        /// <summary>
        /// 字典转Dynamic
        /// </summary>
        /// <param name="dic">字典</param>
        /// <returns></returns>
        public static dynamic ToDynamic(this Dictionary<string, object> dic)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            foreach (var item in dic)
            {
                (result as ICollection<KeyValuePair<string, object>>).Add(new KeyValuePair<string, object>(item.Key, item.Value));
            }
            return result;
        }

        /// <summary>
        /// 转换对象为字典(Dictionary<string, object>转Dictionary<string,string>)
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> ToDictionaryStringValue(Dictionary<string, object> dic)
        {
            var strDic = new Dictionary<string, string>();
            if (dic == null) return strDic;
            foreach (var item in dic)
            {
                if (string.IsNullOrEmpty(item.Key)) continue;
                var key = item.Key;
                object objValue;
                string value;
                try
                {
                    objValue = Convert.ChangeType(item.Value, typeof(string));
                    value = objValue?.ToString();
                }
                catch (Exception)
                {
                    value = JsonConvert.SerializeObject(item.Value);
                }
                strDic.Add(key, value);
            }
            return strDic;
        }

        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, string> ToDictionaryStringValue<T>(this T obj)
        {
            var objDic = obj.ToDictionary();
            return ToDictionaryStringValue(objDic);
        }

    
        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, object> ToDictionary<T>(this T obj)
        {
            return ToDictionary(obj, null, null);
        }

        /// <summary>
        /// 转换对象为字典
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="members">需要转换的成员</param>
        /// <param name="ignoreMembers">忽略转换的成员</param>
        /// <returns>返回字典</returns>
        public static Dictionary<string, object> ToDictionary<T>(this T obj, string[] members, string[] ignoreMembers)
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
                // 判断当前属性是否是索引器属性
                if (propertyInfo.GetIndexParameters().Length > 0)
                {
                    var indexParameters = propertyInfo.GetIndexParameters();
                    // 创建索引参数数组并填充相应的值
                    object[] indexValues = new object[indexParameters.Length]; // 根据实际情况调整数组大小
                    // 获取索引器属性的值并设置到字典中
                    try
                    {
                        // 获取索引器属性的值并设置到字典中
                        dictionary[name] = propertyInfo.GetValue(obj, indexValues);
                    }
                    catch (Exception ex)
                    {
                        // 处理异常
                        // 可以将异常信息输出或进行其他处理
                    }
                }
                else
                {
                    // 获取当前属性的GET访问器
                    var methodInfo = propertyInfo.GetGetMethod();
                    // 判断当前属性的GET访问器是否是公开的
                    if (!methodInfo.IsPublic) continue;
                    // 判断当前属性的GET访问器是否是静态的
                    if (methodInfo.IsStatic)
                    {
                        // 获取成员属性的静态值并设置到字典中
                        dictionary[name] = propertyInfo.GetValue(null);
                    }
                    else
                    {
                        // 获取成员属性的实例值并设置到字典中
                        dictionary[name] = propertyInfo.GetValue(obj);
                    }
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
        public static T ToEntity<T>(this Dictionary<string, object> dictionary) where T : class, new()
        {
            return ToEntity<T>(dictionary, null,null);
        }

        /// <summary>
        /// 转换字典为对象
        /// </summary>
        /// <param name="dictionary">要转换的字典</param>
        /// <param name="members">需要转换的成员</param>
        /// <param name="ignoreMembers">忽略转换的成员</param>
        /// <returns>返回对象</returns>
        public static T ToEntity<T>(this Dictionary<string, object> dictionary, string[] members, string[] ignoreMembers)where T:class,new()
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
