using Grpc.AspNetCore.Server;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;
using System;
using System.Collections.Generic;

namespace UtilsSharp.Grpc.AspNetCore
{
    /// <summary>
    /// 注册Grpc扩展
    /// </summary>
    public static class GrpcExtensions
    {
        /// <summary>
        ///  注册Grpc
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="assemblyNameKeyword">程序集名称关键词</param>
        /// <param name="configureOptions">GrpcServiceOptions</param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcExtensions(this IServiceCollection services,string assemblyNameKeyword,Action<GrpcServiceOptions> configureOptions=null)
        {
            //注册ProtoContract、ProtoInclude、ProtoMember
            services.AddProtobufRunTimeExtensions(assemblyNameKeyword);
            //添加Grpc服务
            services.AddCodeFirstGrpc(configureOptions);
            return services;
        }


        /// <summary>
        ///  注册Grpc
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="marshallerFactories">程序集名称关键词</param>
        /// <param name="configureOptions">GrpcServiceOptions</param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcExtensions(this IServiceCollection services,List<MarshallerFactory> marshallerFactories, Action<GrpcServiceOptions> configureOptions = null)
        {
            //注册ProtoContract、ProtoInclude、ProtoMember
            services.AddProtobufRunTimeExtensions(marshallerFactories);
            //添加Grpc服务
            services.AddCodeFirstGrpc(configureOptions);
            return services;
        }

    }
}
