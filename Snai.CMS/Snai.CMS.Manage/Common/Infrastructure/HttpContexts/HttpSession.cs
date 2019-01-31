using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Infrastructure.HttpContexts
{
    public class HttpSession: IHttpSession
    {
        #region 属性声明

        private readonly IHttpContextAccessor HttpContextAccessor;
        private ISession Session => HttpContextAccessor.HttpContext.Session;

        #endregion

        #region 构造函数

        public HttpSession(IHttpContextAccessor httpContextAccessor)
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

        public T GetSession<T>(string sessionKey)
        {
            string sessionValue = Session.GetString(sessionKey);
            return JsonConvert.DeserializeObject<T>(sessionValue);
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
    }
}
