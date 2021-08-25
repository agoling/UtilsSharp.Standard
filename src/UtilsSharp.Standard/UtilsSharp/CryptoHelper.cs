using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class CryptoHelper
    {
        #region ===MD5加密===
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var md5Algorithm = MD5.Create();
            var utf8Bytes = Encoding.UTF8.GetBytes(str);
            var md5Hash = md5Algorithm.ComputeHash(utf8Bytes);
            var hexString = new StringBuilder();
            foreach (var hexByte in md5Hash)
            {
                hexString.Append(hexByte.ToString("x2"));
            }
            return hexString.ToString();
        }
        #endregion

        #region ===DES加密解密===
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <param name="secretKey">密文</param>
        /// <param name="iv">向量</param>
        /// <returns>加密后的字符串</returns>
        public static string ToDesEncrypt(this string str, string secretKey, string iv)
        {
            if (string.IsNullOrEmpty(str)) return str;
            using var sa= new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(secretKey), IV = Encoding.UTF8.GetBytes(iv) };
            using var ct = sa.CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(str);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, ct,CryptoStreamMode.Write))
            {
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="str">解密字符串</param>
        /// <param name="secretKey">密文</param>
        /// <param name="iv">向量</param>
        /// <returns>解密后的字符串</returns>
        public static string ToDesDecrypt(this string str, string secretKey, string iv)
        {
            if (string.IsNullOrEmpty(str)) return str;
            using var sa =new DESCryptoServiceProvider{Key = Encoding.UTF8.GetBytes(secretKey), IV = Encoding.UTF8.GetBytes(iv)};
            using var ct = sa.CreateDecryptor();
            var bytes = Convert.FromBase64String(str);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
            {
                cs.Write(bytes, 0, bytes.Length);
                cs.FlushFinalBlock();
            }
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion

        #region ===RSA加密解密===

        /// <summary> 
        /// RSA加密 
        /// </summary> 
        /// <param name="str">加密字符串</param> 
        /// <param name="secretKey">密文</param> 
        /// <returns></returns> 
        public static string ToRsaEncrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return str;
            //密匙容器的名称，保持加密解密一致才能解密成功
            var param = new CspParameters {KeyContainerName = secretKey ?? "UtilsSharp"};
            using var rsa = new RSACryptoServiceProvider(param);
            var plaindata = Encoding.Default.GetBytes(str); //将要加密的字符串转换为字节数组
            var encryptdata = rsa.Encrypt(plaindata, false); //将加密后的字节数据转换为新的加密字节数组
            return Convert.ToBase64String(encryptdata); //将加密后的字节数组转换为字符串
        }

        /// <summary> 
        /// RSA解密 
        /// </summary> 
        /// <param name="str">解密字符串</param> 
        /// <param name="secretKey">密匙容器的名称</param> 
        /// <returns></returns> 
        public static string ToRsaDecrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return str;
            //密匙容器的名称，保持加密解密一致才能解密成功
            var param = new CspParameters{KeyContainerName = secretKey ?? "UtilsSharp" };
            using var rsa = new RSACryptoServiceProvider(param);
            var encryptdata = Convert.FromBase64String(str);
            var decryptdata = rsa.Decrypt(encryptdata, false);
            return Encoding.Default.GetString(decryptdata);
        }
        #endregion

        #region ===SHA不可逆加密===
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToSha1Encrypt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var bytes = Encoding.Default.GetBytes(str);
            var sha = new SHA1CryptoServiceProvider();
            var encryptbytes = sha.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }
        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToSha256Encrypt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var bytes = Encoding.Default.GetBytes(str);
            var sha256 = new SHA256CryptoServiceProvider();
            var encryptbytes = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }
        /// <summary>
        /// SHA384加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToSha384Encrypt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var bytes = System.Text.Encoding.Default.GetBytes(str);
            var sha384 = new SHA384CryptoServiceProvider();
            var encryptbytes = sha384.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }
        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <returns></returns>
        public static string ToSha512Encrypt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var bytes = Encoding.Default.GetBytes(str);
            var sha512 = new SHA512CryptoServiceProvider();
            var encryptbytes = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(encryptbytes);
        }
        #endregion

        #region ===Base64加密解密===
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns></returns>
        public static string ToBase64Encrypt(this string str)
        {
            return ToBase64Encrypt(str, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密(指定字符编码)
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string ToBase64Encrypt(this string str, Encoding encode)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return Convert.ToBase64String(encode.GetBytes(str));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns></returns>
        public static string ToBase64Decrypt(this string str)
        {
            return ToBase64Decrypt(str, new UTF8Encoding());
        }

        /// <summary>
        /// Base64解密(指定字符编码)
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string ToBase64Decrypt(this string str, Encoding encode)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return encode.GetString(Convert.FromBase64String(str));
        }
        #endregion

        #region ===AES加密解密===
        /// <summary>
        ///  AES加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="secretKey">密文</param>
        /// <returns></returns>
        public static string ToAesEncrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var toEncryptArray = Encoding.UTF8.GetBytes(str);
            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = rm.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        ///  AES解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="secretKey">密文</param>
        /// <returns></returns>
        public static string ToAesDecrypt(this string str, string secretKey)
        {
            if (string.IsNullOrEmpty(str)) return str;
            var toEncryptArray = Convert.FromBase64String(str);
            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(secretKey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            var cTransform = rm.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion

    }
}
