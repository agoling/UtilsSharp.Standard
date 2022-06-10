using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf;

namespace UtilsSharp.Protobuf
{
    /// <summary>
    /// 序列化和反序列化
    /// </summary>
    public class ProtobufConvert
    {
        /// <summary>
        /// 序列化string类型
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="t">对象</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string Serialize<T>(T t,Encoding encoding=null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return encoding.GetString(ms.ToArray());
            }
        }


        /// <summary>
        /// 序列化 byte
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="t">对象</param>
        /// <returns></returns>
        public static byte[] SerializeByte<T>(T t)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                //定义二级制数组，保存序列化后的结果
                var result = new byte[ms.Length];
                //将流的位置设为0，起始点
                ms.Position = 0;
                //将流中的内容读取到二进制数组中
                ms.Read(result, 0, result.Length);
                return result;
            }
        }


        /// <summary>
        /// 反序列化 string
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="content">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static T DeSerialize<T>(string content, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            using (var ms = new MemoryStream(encoding.GetBytes(content)))
            {
                var t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }


        /// <summary>
        /// 反序列化 T
        /// </summary>
        /// <typeparam name="T">模型</typeparam>
        /// <param name="bytes">byte</param>
        /// <returns></returns>
        public static T DeSerialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                //将消息写入流中
                ms.Write(bytes, 0, bytes.Length);
                //将流的位置归0
                ms.Position = 0;
                //使用工具反序列化对象
                var t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}
