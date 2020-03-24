using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace UtilsSharp
{
    /// <summary>
    /// DataTable帮助类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="table">DataTable实例</param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable table) where T : new()
        {
            var entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (!row.Table.Columns.Contains(item.Name)) continue;
                    if (DBNull.Value == row[item.Name]) continue;
                    var newType = item.PropertyType;
                    //判断type类型是否为泛型，因为nullable是泛型类,
                    if (newType.IsGenericType&& newType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                        var nullableConverter = new System.ComponentModel.NullableConverter(newType);
                        //将type转换为nullable对的基础基元类型
                        newType = nullableConverter.UnderlyingType;
                    }

                    item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                }
            }
            return entity;
        }

        /// <summary>
        ///  DataTable转实体集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="table">DataTable实例</param>
        /// <returns></returns>
        public static List<T> ToEntities<T>(this DataTable table) where T : new()
        {
            var entities = new List<T>();
            if (table == null)
                return null;
            foreach (DataRow row in table.Rows)
            {
                var entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (!table.Columns.Contains(item.Name)) continue;
                    if (DBNull.Value == row[item.Name]) continue;
                    var newType = item.PropertyType;
                    //判断type类型是否为泛型，因为nullable是泛型类,
                    if (newType.IsGenericType&& newType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                        System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);
                        //将type转换为nullable对的基础基元类型
                        newType = nullableConverter.UnderlyingType;
                    }
                    item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                }
                entities.Add(entity);
            }
            return entities;
        }


        /// <summary>
        /// 指定集合转DataTable
        /// </summary>
        /// <param name="list">指定集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IList list)
        {
            var table = new DataTable();
            if (list.Count <= 0) return table;
            var propertys = list[0].GetType().GetProperties();
            foreach (var pi in propertys)
            {
                var pt = pi.PropertyType;
                if (pt.IsGenericType && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    pt = pt.GetGenericArguments()[0];
                }
                table.Columns.Add(new DataColumn(pi.Name, pt));
            }
            foreach (var item in list)
            {
                var tempList = new ArrayList();
                foreach (var pi in propertys)
                {
                    var obj = pi.GetValue(item, null);
                    tempList.Add(obj);
                }
                var array = tempList.ToArray();
                table.LoadDataRow(array, true);
            }
            return table;
        }

        /// <summary>
        /// 指定实体集合转DataTable
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="list">实体集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            var table = new DataTable();
            //创建列头
            var propertys = typeof(T).GetProperties();
            foreach (var pi in propertys)
            {
                var pt = pi.PropertyType;
                if (pt.IsGenericType && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    pt = pt.GetGenericArguments()[0];
                }
                table.Columns.Add(new DataColumn(pi.Name, pt));
            }
            //创建数据行
            if (list.Count <= 0) return table;
            {
                foreach (var item in list)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        var obj = pi.GetValue(item, null);
                        tempList.Add(obj);
                    }
                    var array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }

    }
}
