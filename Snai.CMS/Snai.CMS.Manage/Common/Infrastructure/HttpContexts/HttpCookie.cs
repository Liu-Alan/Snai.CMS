using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snai.CMS.Manage.Common.Infrastructure.HttpContexts
{
    public class HttpCookie: IHttpCookie
    {
        #region 属性声明

        private readonly IHttpContextAccessor HttpContextAccessor;
        private HttpResponse Response => HttpContextAccessor.HttpContext.Response;
        private HttpRequest Request => HttpContextAccessor.HttpContext.Request;

        #endregion

        #region 构造函数

        public HttpCookie(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Cookie操作

        public void SetCookie(string cookieKey, string cookieValue)
        {
            Response.Cookies.Append(cookieKey, cookieValue, new CookieOptions()
            {
                HttpOnly = true,
                Secure = false
            });
        }

        public string GetCookie(string cookieKey)
        {
            Request.Cookies.TryGetValue(cookieKey, out string cookieValue);
            if (!string.IsNullOrEmpty(cookieValue))
            {
                return cookieValue;
            }
            else
            {
                return "";
            }
        }

        public void DelCookie(string cookieKey)
        {
            Response.Cookies.Delete(cookieKey);
        }

        #endregion
    }
}
