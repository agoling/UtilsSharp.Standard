using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 数据计算辅助操作类
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// 计算进度条进度
        /// </summary>
        /// <param name="step">增加的量</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="completeProgress">总完成进度</param>
        /// <returns></returns>
        public static int CalcProgress(int step, int totalCount, int completeProgress = 100)
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
        /// 获取两个坐标的距离
        /// </summary>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

    }
}
