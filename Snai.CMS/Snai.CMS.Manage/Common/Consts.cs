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

        // 登录
        public const string Url_AdminLogin = "/Login/AdminLogin";

        // 首页
        public const string Url_ManageIndex = "/Home/Index";

        #endregion

        #region 视图地址

        // 无权限视图
        public const string View_NoUserRole = "NoUserRole";

        #endregion

        #region 分页信息

        //每页显示数
        public const int Page_Limit = 20;

        #endregion
    }
}
