using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models;
using Snai.CMS.Manage.Models.Home;

namespace Snai.CMS.Manage.Controllers
{
    public class HomeController : Controller
    {
        #region 属性声明

        IOptions<WebSettings> WebSettings;
        ICMSAdminBO CMSAdminBO;
        ICMSAdminCookie CMSAdminCookie;

        #endregion

        #region 构造函数

        public HomeController(IOptions<WebSettings> webSettings, ICMSAdminBO cmsAdminBO, ICMSAdminCookie cmsAdminCookie)
        {
            WebSettings = webSettings;
            CMSAdminBO = cmsAdminBO;
            CMSAdminCookie = cmsAdminCookie;
        }

        #endregion

        // 首页
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult Index()
        {
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new IndexModel
            {
                PageTitle = module == null ? "" : module.Title,
                LastLogonIP = "本机IP",
                LastLogonTime = DateTime.Now,
                WebTitle = WebSettings.Value.WebTitle
            };

            var adminToken = CMSAdminCookie.GetAdiminCookie();
            if (adminToken != null && !string.IsNullOrEmpty(adminToken.UserName))
            {
                var admin = CMSAdminBO.GetAdminByUserName(adminToken.UserName);
                if (admin != null && !string.IsNullOrEmpty(admin.UserName))
                {
                    model.LastLogonIP = admin.LastLogonIP;
                    model.LastLogonTime = DateTimeUtils.UnixTimeStampToDateTime(admin.LastLogonTime);
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

        // 登录信息
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult LoginInfo()
        {
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new IndexModel
            {
                PageTitle = module == null ? "" : module.Title,
                LastLogonIP = "本机IP",
                LastLogonTime = DateTime.Now,
                WebTitle = WebSettings.Value.WebTitle
            };

            // 取菜单和登录信息
            var adminToken = CMSAdminCookie.GetAdiminCookie();
            if (adminToken != null && !string.IsNullOrEmpty(adminToken.UserName))
            {
                var admin = CMSAdminBO.GetAdminByUserName(adminToken.UserName);
                if (admin != null && !string.IsNullOrEmpty(admin.UserName))
                {
                    model.LastLogonIP = admin.LastLogonIP;
                    model.LastLogonTime = DateTimeUtils.UnixTimeStampToDateTime(admin.LastLogonTime);
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

            return View("./Index", model);
        }

        // 修改密码
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult UpdatePassword()
        {
            var module = CMSAdminBO.GetModule(ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);

            var model = new UpdatePasswordModel
            {
                PageTitle = module == null ? "" : module.Title,
                WebTitle = WebSettings.Value.WebTitle
            };

            // 取菜单和用户名
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

        [ServiceFilter(typeof(AuthorizationFilter))]
        public ActionResult<Message> DoUpdatePassword()
        {
            var msg = new Message(10, "修改密码失败！");

            string oldPassword = Request.Form["oldPassword"];
            string password = Request.Form["password"];
            string rePassword = Request.Form["rePassword"];

            var adminToken = CMSAdminCookie.GetAdiminCookie();
            var admin = CMSAdminBO.GetAdminByUserName(adminToken.UserName);
            if (admin != null || admin.ID > 0)
            {
                msg = CMSAdminBO.UpdatePasswordByID(admin.ID, oldPassword, password, rePassword);
            }

            return new JsonResult(msg);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
