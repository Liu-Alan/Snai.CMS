using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Infrastructure.Extension;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Common.Infrastructure.ValidateCodes;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models;
using Snai.CMS.Manage.Models.Home;

namespace Snai.CMS.Manage.Controllers
{
    public class HomeController : ControllerBase
    {
        #region 构造函数

        public HomeController(IOptions<WebSettings> webSettings, IValidateCode validateCode, HttpContextExtension httpExtension, ICMSAdminBO cmsAdminBO, ICMSAdminCookie cmsAdminCookie)
            : base(webSettings, validateCode, httpExtension, cmsAdminBO, cmsAdminCookie)
        {
        }

        #endregion

        // 首页
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult Index()
        {
            // 权限和菜单
            IndexModel model = new IndexModel();
            var layoutModel = this.GetLayoutModel();
            if (layoutModel != null)
            {
                layoutModel.ToT(ref model);
            }

            var admin = CMSAdminBO.GetAdminByUserName(model.UserName);
            if (admin != null && !string.IsNullOrEmpty(admin.UserName))
            {
                model.LastLogonIP = admin.LastLogonIP;
                model.LastLogonTime = DateTimeUtils.UnixTimeStampToDateTime(admin.LastLogonTime);
            }
            else
            {
                model.LastLogonIP = "本机IP";
                model.LastLogonTime = DateTime.Now;
            }

            return View(model);
        }

        // 登录信息
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult LoginInfo()
        {
            // 权限和菜单
            IndexModel model = new IndexModel();
            var layoutModel = this.GetLayoutModel();
            if (layoutModel != null)
            {
                layoutModel.ToT(ref model);
            }

            var admin = CMSAdminBO.GetAdminByUserName(model.UserName);
            if (admin != null && !string.IsNullOrEmpty(admin.UserName))
            {
                model.LastLogonIP = admin.LastLogonIP;
                model.LastLogonTime = DateTimeUtils.UnixTimeStampToDateTime(admin.LastLogonTime);
            }
            else
            {
                model.LastLogonIP = "本机IP";
                model.LastLogonTime = DateTime.Now;
            }

            return View("./Index", model);
        }

        // 修改密码
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult UpdatePassword()
        {
            //展示页面
            if (!Request.Method.ToUpper().Equals("POST", StringComparison.OrdinalIgnoreCase) || !Request.HasFormContentType)
            {
                // 权限和菜单
                UpdatePasswordModel model = new UpdatePasswordModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                return View(model);
            }
            else
            {
                //修改密码
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
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
