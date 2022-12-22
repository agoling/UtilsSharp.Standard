using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace UtilsSharp.Grpc.ClientFactory
{
    /// <summary>
    /// GrpcHelper
    /// </summary>
    public class GrpcHelper
    {

        /// <summary>
        /// 获取GrpcChannel
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="allowUnencryptedHttp2">如果服务端没有加密传输，客户端必须设置为true</param>
        /// <returns></returns>
        public static GrpcChannel GetChannel(string address,bool allowUnencryptedHttp2=false)
        {
            //如果服务端没有加密传输，客户端必须设置
            GrpcClientFactory.AllowUnencryptedHttp2 = allowUnencryptedHttp2;
            return GrpcChannel.ForAddress(address);
        }

        /// <summary>
        /// 获取GrpcChannel
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="channelOptions">channelOptions</param>
        /// <param name="allowUnencryptedHttp2">如果服务端没有加密传输，客户端必须设置为true</param>
        /// <returns></returns>
        public static GrpcChannel GetChannel(string address, GrpcChannelOptions channelOptions,bool allowUnencryptedHttp2 = false)
        {
            //如果服务端没有加密传输，客户端必须设置
            GrpcClientFactory.AllowUnencryptedHttp2 = allowUnencryptedHttp2;
            return GrpcChannel.ForAddress(address, channelOptions);
        }


        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="TService">接口服务</typeparam>
        /// <param name="address">address</param>
        /// <param name="allowUnencryptedHttp2">如果服务端没有加密传输，客户端必须设置为true</param>
        /// <returns></returns>
        public static TService GetService<TService>(string address, bool allowUnencryptedHttp2 = false) where TService : class
        {
            var channel = GetChannel(address, allowUnencryptedHttp2);
            var clientFactory = ProtobufRunTime.GetClientFactory<TService>();
            var service = channel.CreateGrpcService<TService>(clientFactory);
            return service;
        }


        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="TService">接口服务</typeparam>
        /// <param name="address">address</param>
        /// <param name="channelOptions">channelOptions</param>
        /// <param name="allowUnencryptedHttp2">如果服务端没有加密传输，客户端必须设置为true</param>
        /// <returns></returns>
        public static TService GetService<TService>(string address, GrpcChannelOptions channelOptions, bool allowUnencryptedHttp2 = false) where TService : class
        {
            var channel = GetChannel(address, channelOptions,allowUnencryptedHttp2);
            var clientFactory = ProtobufRunTime.GetClientFactory<TService>();
            var service = channel.CreateGrpcService<TService>(clientFactory);
            return service;
        }


        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="TService">接口服务</typeparam>
        /// <param name="channel">如果服务端没有加密传输，客户端必须设置为true</param>
        /// <returns></returns>
        public static TService GetService<TService>(GrpcChannel channel) where TService : class
        {
            var clientFactory = ProtobufRunTime.GetClientFactory<TService>();
            var service = channel.CreateGrpcService<TService>(clientFactory);
            return service;
        }

    }
}
