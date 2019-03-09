using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
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

        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult Index()
        {
            Console.WriteLine("Action页面开始");
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
                    model.RoleTitle = role != null ? role.Title : "";
                }
            }

            Console.WriteLine("Action页面结束");

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
