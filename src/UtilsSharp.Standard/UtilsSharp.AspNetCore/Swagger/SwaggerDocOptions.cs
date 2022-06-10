using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using Microsoft.OpenApi.Models;

namespace UtilsSharp.AspNetCore.Swagger
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
        /// 项目名称
        /// </summary>
        public string ProjectName { set; get; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDescription { set; get; }

        /// <summary>
        /// 项目版本号
        /// </summary>
        public string ProjectVersion { set; get; }

        /// <summary>
        /// Header默认值
        /// </summary>
        public List<HeaderParameter> HeaderParameters { set; get; }=new List<HeaderParameter>();

        /// <summary>
        /// swagger分组
        /// </summary>
        public List<SwaggerGroup> Groups { set; get; }=new EditableList<SwaggerGroup>();
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

    /// <summary>
    /// swagger分组
    /// </summary>
    public class SwaggerGroup: OpenApiInfo
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { set; get; }
    }
}
