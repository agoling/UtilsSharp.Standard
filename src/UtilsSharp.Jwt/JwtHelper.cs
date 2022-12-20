using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp.Jwt
{
    /// <summary>
    /// Jwt帮助类
    /// </summary>
    public class JwtHelper
    {

        /// <summary>
        /// JwtSecurityTokenHandler
        /// </summary>
        public static JwtSecurityTokenHandler JwtSth = new JwtSecurityTokenHandler();

        /// <summary>
        /// 生成JwtToken
        /// </summary>
        /// <param name="jwtSetting">Jwt参数</param>
        /// <param name="claims">Payload||存放用户信息</param>
        /// <returns></returns>
        public static BaseResult<string> Create(JwtSetting jwtSetting, Claim[] claims)
        {
            var result = new BaseResult<string>();
            try
            {
                //Header,选择签名算法
                var signingAlgorithm = SecurityAlgorithms.HmacSha256;
                //Signature
                //取出私钥并以utf8编码字节输出
                var secretByte = Encoding.UTF8.GetBytes(jwtSetting.SecretKey);
                //使用非对称算法对私钥进行加密
                var signingKey = new SymmetricSecurityKey(secretByte);
                //使用HmacSha256来验证加密后的私钥生成数字签名
                var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);
                //生成Token
                var token = new JwtSecurityToken(
                    issuer: jwtSetting.Issuer, //发布者
                    audience: jwtSetting.Audience,//接收者
                    claims: claims,  //Payload,存放用户信息
                    notBefore: DateTime.UtcNow,//发布时间
                    expires: DateTime.UtcNow.AddSeconds(jwtSetting.ExpireTime),//有效期设置为秒
                    signingCredentials //数字签名
                );
                //生成字符串token
                var jwtToken= JwtSth.WriteToken(token);
                result.Result = $"{JwtBearerDefaults.AuthenticationScheme} {jwtToken}";

            }
            catch (Exception ex)
            {
                result.SetError(ex.Message,5000);
            }
            return result;
        }

        /// <summary>
        /// 获取用户信息Payload内容
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static BaseResult<JwtPayload> GetPayload(string token)
        {
            var result = new BaseResult<JwtPayload>();
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    result.SetError("token不能为空！");
                    return result;
                }
                token = Regex.Replace(token, "Bearer\\s+", "");
                var jwtToken = JwtSth.ReadJwtToken(token);
                result.Result = jwtToken.Payload;
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
                return result;
            }
        }

        /// <summary>
        /// 获取用户信息Payload内容
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static BaseResult<JwtSecurityToken> GetTokenInfo(string token)
        {
            var result = new BaseResult<JwtSecurityToken>();
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    result.SetError("token不能为空！");
                    return result;
                }
                token= Regex.Replace(token, "Bearer\\s+", "");
                result.Result = JwtSth.ReadJwtToken(token);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message);
                return result;
            }
        }

    }
}
