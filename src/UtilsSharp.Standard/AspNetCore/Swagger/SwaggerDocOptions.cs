using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;

namespace AspNetCore.Swagger
{
    /// <summary>
    /// SwaggerDocOptions
    /// </summary>
    public class SwaggerDocOptions
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { set; get; } = "v1";

        /// <summary>
        /// OpenApiInfo
        /// </summary>
        public OpenApiInfo OpenApiInfo { set; get; } = new OpenApiInfo {Title = "My API", Version = "v1"};
    }
}
