using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Snai.CMS.Manage.Common.Infrastructure.Extension
{
    public class HttpContextExtension
    {
        #region 属性声明

        private readonly IHttpContextAccessor HttpContextAccessor;
        private HttpContext Context => HttpContextAccessor.HttpContext;
        private ISession Session => Context.Session;
        private HttpResponse Response => Context.Response;
        private HttpRequest Request => Context.Request;

        #endregion

        #region 构造函数

        public HttpContextExtension(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Session操作

        public void SetSession(string sessionKey, string sessionValue)
        {
            Session.SetString(sessionKey, sessionValue);
        }

        public string GetSession(string sessionKey)
        {
            return Session.GetString(sessionKey);
        }

        public void RemoveSession(string sessionKey)
        {
            Session.Remove(sessionKey);
        }

        public bool EqualsSessionValue(string sessionKey, string equalsValue)
        {
            string sessionValue = Session.GetString(sessionKey);

            return string.Equals(equalsValue, sessionValue, StringComparison.OrdinalIgnoreCase);
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

        #region 常用方法

        //取客户端IP
        public string GetUserIP()
        {
            var ip = Context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = Context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        #endregion
    }
}
