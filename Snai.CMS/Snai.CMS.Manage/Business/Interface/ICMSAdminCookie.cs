using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Business.Interface
{
    public interface ICMSAdminCookie
    {
        #region AdminCookie操作

        //设置Cookie
        void SetAdiminCookie(AdminLogin admin);

        //读取Cookie
        AdminToken GetAdiminCookie();

        //删除Cookie
        void DelAdiminCookie();

        #endregion
    }
}
