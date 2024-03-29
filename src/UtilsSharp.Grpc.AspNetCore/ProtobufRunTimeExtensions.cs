﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Configuration;

namespace UtilsSharp.Grpc.AspNetCore
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
            var marshallerFactories = ProtobufRunTime.GetMarshallerFactoryByKeyword(assemblyNameKeyword);
            if (marshallerFactories==null||marshallerFactories.Count == 0)
            {
                return services;
            }
            services.TryAddSingleton(BinderConfiguration.Create(marshallerFactories));
            return services;
        }


        /// <summary>
        ///  注册ProtoContract、ProtoInclude、ProtoMember
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="marshallerFactories">程序集名称关键词</param>
        /// <returns></returns>
        public static IServiceCollection AddProtobufRunTimeExtensions(this IServiceCollection services,List<MarshallerFactory> marshallerFactories)
        {
            if (marshallerFactories == null || marshallerFactories.Count == 0)
            {
                return services;
            }
            services.TryAddSingleton(BinderConfiguration.Create(marshallerFactories));
            return services;
        }
    }
}
