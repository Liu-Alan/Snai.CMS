using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Infrastructure.HttpContexts
{
    public interface IHttpCookie
    {
        #region Cookie操作

        void SetCookie(string cookieKey, string cookieValue);

        string GetCookie(string cookieKey);

        void DelCookie(string cookieKey);

        #endregion
    }
}
