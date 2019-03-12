using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Infrastructure.Extension
{
    public interface IHttpContextExtension
    {
        #region Session操作

        void SetSession(string sessionKey, string sessionValue);

        string GetSession(string sessionKey);

        void RemoveSession(string sessionKey);

        bool EqualsSessionValue(string sessionKey, string equalsValue);

        #endregion

        #region Cookie操作

        void SetCookie(string cookieKey, string cookieValue);

        string GetCookie(string cookieKey);

        void DelCookie(string cookieKey);

        #endregion

        #region 常用方法

        string GetUserIP();

        #endregion
    }
}
