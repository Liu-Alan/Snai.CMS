using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models.BackManage;

namespace Snai.CMS.Manage.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class BackManageController : Controller
    {
        #region 属性声明

        IOptions<WebSettings> WebSettings;
        ICMSAdminBO CMSAdminBO;
        ICMSAdminCookie CMSAdminCookie;

        #endregion

        #region 构造函数

        public BackManageController(IOptions<WebSettings> webSettings, ICMSAdminBO cmsAdminBO, ICMSAdminCookie cmsAdminCookie)
        {
            WebSettings = webSettings;
            CMSAdminBO = cmsAdminBO;
            CMSAdminCookie = cmsAdminCookie;
        }

        #endregion

        public IActionResult Index()
        {
            // 权限和菜单
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new IndexModel
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

            return View(model);
        }

        public IActionResult AdminList()
        {
            // 权限和菜单
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new AdminListModel
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

            //取管理员列表分布
            model.UserNameFilter = Request.Query["userName"];
            var roleID = 0;
            int.TryParse( Request.Query["roleID"], out roleID);
            model.RoleIDFilter = roleID;

            model.PageLimit = Consts.Page_Limit;
            var admins = CMSAdminBO.GetAdmins(model.UserNameFilter, model.RoleIDFilter);
            if (admins != null)
            {
                model.Admins= admins.ToList();
            }
            

            return View(model);
        }
    }
}