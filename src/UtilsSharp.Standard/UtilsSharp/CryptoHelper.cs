using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class CryptoHelper
    {
        /// <summary>
        /// 转换为MD5
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static string ToMd5(this string origin)
        {
            if (string.IsNullOrWhiteSpace(origin))
            {
                return string.Empty;
            }

            var md5Algorithm = MD5.Create();
            var utf8Bytes = Encoding.UTF8.GetBytes(origin);
            var md5Hash = md5Algorithm.ComputeHash(utf8Bytes);
            var hexString = new StringBuilder();
            foreach (var hexByte in md5Hash)
            {
                hexString.Append(hexByte.ToString("x2"));
            }
            return hexString.ToString();
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="secretKey">密文</param>
        /// <returns></returns>
        public static string ToAesEncrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            RijndaelManaged rm = new RijndaelManaged
            {
                Key = keyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="secretKey">密文</param>
        /// <returns></returns>
        public static string ToAesDecrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            byte[] pwdBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = keyBytes,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
