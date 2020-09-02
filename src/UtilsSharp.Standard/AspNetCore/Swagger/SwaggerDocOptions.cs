using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace AspNetCore.Swagger
{
    /// <summary>
    /// SwaggerDocOptions
    /// </summary>
    public class SwaggerDocOptions
    {
        /// <summary>
        /// 生产环境可禁用Swagger
        /// </summary>
        public bool Enable { set; get; } = true;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { set; get; } = "v1";

        /// <summary>
        /// OpenApiInfo
        /// </summary>
        public OpenApiInfo OpenApiInfo { set; get; } = new OpenApiInfo {Title = "My API", Version = "v1"};

        /// <summary>
        /// Header默认值
        /// </summary>
        public List<HeaderParameter> HeaderParameters { set; get; }=new List<HeaderParameter>();
    }


    /// <summary>
    /// Header参数
    /// </summary>
    public class HeaderParameter
    {
        /// <summary>
        /// Header名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Header默认值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// Header描述
        /// </summary>
        public string Description { set; get; }
    }
}
