using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common.Infrastructure.Extension;
using Snai.CMS.Manage.Common.Infrastructure.ValidateCodes;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models;

namespace Snai.CMS.Manage.Controllers
{
    public class ControllerBase : Controller
    {
        #region 属性声明

        protected IOptions<WebSettings> WebSettings;
        protected IValidateCode ValidateCode;
        protected HttpContextExtension HttpExtension;
        protected ICMSAdminBO CMSAdminBO;
        protected ICMSAdminCookie CMSAdminCookie;

        #endregion

        #region 构造函数

        public ControllerBase(IOptions<WebSettings> webSettings, IValidateCode validateCode, HttpContextExtension httpExtension, ICMSAdminBO cmsAdminBO, ICMSAdminCookie cmsAdminCookie)
        {
            WebSettings = webSettings;
            ValidateCode = validateCode;
            HttpExtension = httpExtension;
            CMSAdminBO = cmsAdminBO;
            CMSAdminCookie = cmsAdminCookie;
        }

        #endregion

        public LayoutModel GetLayoutModel()
        {
            // 权限和菜单
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new LayoutModel
            {
                PageTitle = module == null ? "" : module.Title,
                WebTitle = WebSettings.Value.WebTitle
            };

            var adminToken = CMSAdminCookie.GetAdiminCookie();
            if (adminToken != null && !string.IsNullOrEmpty(adminToken.UserName))
            {
                var admin = CMSAdminBO.GetAdminByUserName(adminToken.UserName);
                if (admin != null && !string.IsNullOrEmpty(admin.UserName))
                {
                    model.UserName = admin.UserName;

                    var role = CMSAdminBO.GetRoleByID(admin.RoleID);
                    if (role != null && role.ID > 0)
                    {
                        model.RoleTitle = role.Title;
                        var roleModules = CMSAdminBO.GetModulesByRoleID(role.ID);
                        if (roleModules != null)
                        {
                            model.RoleModules = roleModules.ToList();
                        }

                        if (module != null && module.ID > 0)
                        {
                            var thisModules = CMSAdminBO.GetThisModuleIDs(model.RoleModules, module.ID);
                            if (thisModules != null)
                            {
                                model.ThisModules = thisModules.ToList();
                            }
                        }
                    }
                }
            }

            return model;
        }
    }
}
