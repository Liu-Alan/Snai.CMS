using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Snai.CMS.Manage.Business.Interface;

namespace Snai.CMS.Manage.Common.Infrastructure.Filters
{
    public class AuthorizationFilter: IActionFilter
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

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var login = CMSAdminBO.VerifyAdminLogin();
            if (!login)
            {
                context.Result = new RedirectResult("/Login/AdminLogin");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
