using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common
{
    //常数定义
    public sealed class Consts
    {
        #region 验证码常数

        // 验证码 Session Key
        public const string Session_ValidateCode = "session_validatecode";

        #endregion

        #region 登录Cookie常数

        // 登录 Cookie Key
        public const string Cookie_AdminToken = "cookie_mgtoken";

        #endregion

        #region 常用URL

        public const string Url_AdminLogin = "/Login/AdminLogin";

        public const string Url_ManageIndex = "/Home/Index";

        #endregion

        #region 视图地址

        public const string View_NoRoleRight = "NoRoleRight"; 

        #endregion
    }
}
