﻿using System;
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
        /// 是否开启Authorization
        /// </summary>
        public bool EnableAuthorization { set; get; } = false;
        /// <summary>
        /// Authorization  SecurityName
        /// </summary>
        public string SecurityName { set; get; }
        /// <summary>
        /// Authorization  SecurityScheme
        /// </summary>
        public OpenApiSecurityScheme SecurityScheme { set; get; }
        /// <summary>
        /// Authorization  SecurityRequirement
        /// </summary>
        public OpenApiSecurityRequirement SecurityRequirement { set; get; }

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
        /// 参数的位置:可能的值为“query”、“header”、“path”或“cookie”。
        /// </summary>
        public ParameterLocation? In { set; get; } = ParameterLocation.Header;
        /// <summary>
        /// Header默认值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// Header描述
        /// </summary>
        public string Description { set; get; }
        /// <summary>
        /// 是否是必须的
        /// </summary>
        public bool Required { set; get; } = false;

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
