﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// 基础分页参数
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { set; get; }
    }
}
