using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Configuration;

namespace UtilsSharp.Protobuf
{
    /// <summary>
    /// 注册ProtoContract、ProtoInclude、ProtoMember扩展
    /// </summary>
    public static class ProtobufRunTimeExtensions
    {
        /// <summary>
        ///  注册ProtoContract、ProtoInclude、ProtoMember
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="assemblyNameKeyword">程序集名称关键词</param>
        /// <returns></returns>
        public static IServiceCollection AddProtobufRunTimeExtensions(this IServiceCollection services,string assemblyNameKeyword)
        {
            var clientFactory = ProtobufRunTime.GetMarshallerFactoryByKeyword(assemblyNameKeyword);
            services.TryAddSingleton(BinderConfiguration.Create(clientFactory));
            return services;
        }
    }
}
