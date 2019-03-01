using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Snai.CMS.Manage.Business.Interface;

namespace Snai.CMS.Manage.Common.Infrastructure.Filters
{
    public class AuthorizationFilter: IAuthorizationFilter
    {
        #region 属性声明

        public ICMSAdminBO CMSAdminBO;

        #endregion

        #region 构造函数

        public AuthorizationFilter(ICMSAdminBO cmsAdminBO)
        {
            CMSAdminBO = cmsAdminBO;
        }
        
        #endregion

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //判断是否登录
            var login = CMSAdminBO.VerifyAdminLogin();
            if (!login)
            {
                context.Result = new RedirectResult(Consts.Url_AdminLogin);
            }

            //判断是否有权限
            var controller =  context.RouteData.Values["Controller"].ToString();
            var action = context.RouteData.Values["Action"].ToString();

            bool roleRight = false;
            if (!roleRight)
            {
                context.Result = new ViewResult() { ViewName = Consts.View_NoRoleRight };
            }
        }
    }
}
