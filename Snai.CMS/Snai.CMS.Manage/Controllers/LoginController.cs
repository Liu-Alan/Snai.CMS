using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Infrastructure.HttpContexts;
using Snai.CMS.Manage.Common.Infrastructure.ValidateCodes;
using Snai.CMS.Manage.Entities.BackManage;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models;

namespace Snai.CMS.Manage.Controllers
{
    public class LoginController : Controller
    {
        #region 属性声明

        IOptions<WebSettings> WebSettings;
        IValidateCode ValidateCode;
        IHttpSession HttpSession;
        ICMSAdminBO CMSAdminBO;

        #endregion

        #region 构造函数

        public LoginController(IOptions<WebSettings> webSettings, IValidateCode validateCode, IHttpSession httpSession, ICMSAdminBO cmsAdminBO)
        {
            WebSettings = webSettings;
            ValidateCode = validateCode;
            HttpSession = httpSession;
            CMSAdminBO = cmsAdminBO;
        }

        #endregion

        #region 登录

        public IActionResult AdminLogin()
        {
            var model = new AdminLoginModel()
            {
                WebTitle = WebSettings.Value.WebTitle
            };

            return View();
        }

        public ActionResult<Message> DoLogin()
        {
            string userName = Request.Form["userName"];
            string password = Request.Form["password"];
            string validateCode = Request.Form["validateCode"];

            var admin = new AdminLogin
            {
                UserName = userName,
                Password = password,
                ValidateCode = validateCode
            };

            var msg = CMSAdminBO.AdminLogin(admin);

            if (msg.Success)
            {
                msg.Msg = "/Home/Index";
            }

            return new JsonResult(msg);
        }

        #endregion

        #region 登出

        public ActionResult<Message> AdminLogout()
        {
            CMSAdminBO.AdminLogout();

            var msg=new Message(0, "/Login/AdminLogin");

            return new JsonResult(msg);
        }

        #endregion

        #region 验证码

        public IActionResult VerifyCode(string identify = "")
        {
            string codeValue = "";
            var codeImg = ValidateCode.CreateImage(out codeValue, 6);
            codeValue = codeValue.ToUpper();//验证码不分大小写  

            HttpSession.SetSession(Consts.Session_ValidateCode, codeValue);

            Response.Body.Dispose();
            return File(codeImg, @"image/png");
        }

        #endregion

    }
}