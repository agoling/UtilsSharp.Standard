using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using UtilsSharp.Entity;

namespace UtilsSharp
{
    /// <summary>
    /// 公共工具类
    /// </summary>
    public class UtilsHelper
    {
        private UtilsHelper()
        {

        }
        private static readonly UtilsHelper Current = new UtilsHelper();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public static UtilsHelper CreateInstance()
        {
            return Current;
        }

        /// <summary>
        /// 计算进度条进度
        /// </summary>
        /// <param name="step">增加的量</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="completeProgress">总完成进度</param>
        /// <returns></returns>
        public int CalcProgress(int step, int totalCount, int completeProgress = 100)
        {
            try
            {
                var curCount = step;
                var progress = (int)(curCount * 1.0 / totalCount * completeProgress);
                if (step == totalCount)
                {
                    progress = completeProgress;
                }
                return progress;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 反射对象深度拷贝
        /// </summary>
        /// <typeparam name="T">对象模型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public T DeepCopy<T>(T obj)
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

        /// <summary>
        /// 枚举转List
        /// </summary>
        /// <typeparam name="T">枚举对象</typeparam>
        /// <returns></returns>
        public List<EnumEntity> EnumToList<T>()
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
