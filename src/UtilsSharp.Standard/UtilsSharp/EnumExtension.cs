using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using UtilsSharp.Entity;

namespace UtilsSharp
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 字符串转换为枚举
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">枚举值不区分(字符串)</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// 数值转换为枚举
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">枚举值(数值)</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        /// <summary>
        /// 判断某个值是否定义在枚举中
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsDefined<T>(this object value) where T : Enum
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="en">枚举对象</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum en)
        {
            Type type = en.GetType();
            FieldInfo fd = type.GetField(en.ToString());
            if (fd == null)return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }

        /// <summary>
        /// 枚举转List
        /// </summary>
        /// <typeparam name="T">枚举对象</typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>()
        {
            var list = new List<EnumEntity>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                var model = new EnumEntity();
                var objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr.Length > 0)
                {
                    var da = objArr[0] as DescriptionAttribute;
                    if (da != null) model.Description = da.Description;
                }
                model.EnumValue = Convert.ToInt32(e);
                model.EnumName = e.ToString();
                list.Add(model);
            }
            return list;
        }
    }
}
