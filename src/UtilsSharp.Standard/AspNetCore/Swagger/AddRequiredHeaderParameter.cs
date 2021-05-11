using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Swagger
{
    /// <summary>
    /// 添加Header配置
    /// </summary>
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            if(AspNetCoreExtensionsConfig.SwaggerDocOptions==null) return;
            if (AspNetCoreExtensionsConfig.SwaggerDocOptions.HeaderParameters == null || AspNetCoreExtensionsConfig.SwaggerDocOptions.HeaderParameters.Count <= 0) return;
            foreach (var item in AspNetCoreExtensionsConfig.SwaggerDocOptions.HeaderParameters)
            {
                var param = new OpenApiParameter
                {
                    Name = item.Name,
                    In = ParameterLocation.Header,
                    Description = item.Description
                };
                if (!string.IsNullOrEmpty(item.Value))
                {
                    param.Schema = new OpenApiSchema
                    {
                        Type = "String",
                        Default = new OpenApiString(item.Value)
                    };
                }
                operation.Parameters.Add(param);
            }
        }
    }
}
