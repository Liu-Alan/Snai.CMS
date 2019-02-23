using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Infrastructure.HttpContexts
{
    public interface IHttpSession
    {
        #region Session操作

        void SetSession(string sessionKey, string sessionValue);

        string GetSession(string sessionKey);

        void RemoveSession(string sessionKey);

        bool EqualsSessionValue(string sessionKey, string equalsValue);

        #endregion
    }
}
