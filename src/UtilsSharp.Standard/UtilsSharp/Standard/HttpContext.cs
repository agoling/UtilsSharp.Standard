using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace UtilsSharp.Standard
{
    /// <summary>
    /// HttpContext
    /// </summary>
    public static class HttpContext
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Current
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current => _httpContextAccessor.HttpContext;

        /// <summary>
        /// HttpContext
        /// </summary>
        public static void Register(IHttpContextAccessor ahttpContextAccessor)
        {
            _httpContextAccessor = ahttpContextAccessor;
        }
    }
}
