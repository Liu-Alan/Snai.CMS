using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Infrastructure.Extension;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Common.Infrastructure.ValidateCodes;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.Entities.BackManage;
using Snai.CMS.Manage.Entities.Settings;
using Snai.CMS.Manage.Models.BackManage;

namespace Snai.CMS.Manage.Controllers
{
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class BackManageController : ControllerBase
    {
        #region 构造函数

        public BackManageController(IOptions<WebSettings> webSettings, IValidateCode validateCode, HttpContextExtension httpExtension, ICMSAdminBO cmsAdminBO, ICMSAdminCookie cmsAdminCookie)
            : base(webSettings, validateCode, httpExtension, cmsAdminBO, cmsAdminCookie)
        {
        }

        #endregion

        public IActionResult Index()
        {
            // 权限和菜单
            IndexModel model = new IndexModel();
            var layoutModel = this.GetLayoutModel();
            if (layoutModel != null)
            {
                layoutModel.ToT(ref model);
            }

            return View(model);
        }

        #region 账号管理

        //账号管理
        public IActionResult AdminList(string id)
        {
            if (id == null || !id.ToUpper().Equals("DATA", StringComparison.OrdinalIgnoreCase))
            {
                // 权限和菜单
                AdminListModel model = new AdminListModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                var roles = CMSAdminBO.GetRoles(0);
                if (roles != null)
                {
                    model.Roles = roles.ToList();
                }

                return View(model);
            }
            else
            {
                //取管理员列表分布
                string userNameFilter = Request.Query["userName"];

                int roleIDFilter = 0;
                int.TryParse(Request.Query["roleID"], out roleIDFilter);

                int pageIndex = 0;
                int.TryParse(Request.Query["page"], out pageIndex);

                int pageLimit = Consts.Page_Limit;
                int totCount = CMSAdminBO.GetAdminCount(userNameFilter, roleIDFilter);
                int pageCount = (int)Math.Ceiling(totCount / (float)pageLimit);
                var admins = new List<Admin>();
                if (totCount > 0)
                {
                    IEnumerable<Admin> adminIE = CMSAdminBO.GetAdmins(userNameFilter, roleIDFilter, pageLimit, pageIndex);
                    if (adminIE != null)
                    {
                        admins = adminIE.ToList();
                    }
                }

                dynamic model = new ExpandoObject();

                model.code = 0;
                model.msg = "";
                model.count = totCount;
                model.data = admins.Select(s => new
                {
                    id = s.ID,
                    userName = s.UserName,
                    roleTitle = s.RoleTitle,
                    state = s.State,
                    lockState = s.LockState
                });

                return new JsonResult(model);
            }

        }

        //添加修改账号
        public IActionResult ModifyAdmin()
        {
            //展示页面
            if (!Request.Method.ToUpper().Equals("POST", StringComparison.OrdinalIgnoreCase) || !Request.HasFormContentType)
            {
                // 权限和菜单
                ModifyAdminModel model = new ModifyAdminModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                var roles = CMSAdminBO.GetRoles(0);
                if (roles != null)
                {
                    model.Roles = roles.ToList();
                }

                int id = 0;
                int.TryParse(Request.Query["id"], out id);

                if (id > 0)
                {
                    model.PageTitle = "修改账号";
                    var admin = CMSAdminBO.GetAdminByID(id);
                    if (admin != null && admin.ID > 0)
                    {
                        model.Admin = admin;
                    }
                }
                else
                {
                    model.PageTitle = "添加账号";
                }

                return View(model);
            }
            else
            {
                var msg = new Message(10, "修改失败！");

                int id = 0;
                int.TryParse(Request.Form["id"], out id);
                string userName = Request.Form["userName"];
                string password = Request.Form["password"];
                string rePassword = Request.Form["rePassword"];
                int roleID = 0;
                int.TryParse(Request.Form["roleID"], out roleID);
                byte state = 0;
                byte.TryParse(Request.Form["state"], out state);

                var admin = new Admin()
                {
                    ID = id,
                    UserName = userName,
                    Password = password,
                    RePassword = rePassword,
                    RoleID = roleID,
                    State = state
                };

                if (admin.ID > 0)
                {
                    msg = CMSAdminBO.UpdateAdminByID(admin);
                }
                else
                {
                    msg = CMSAdminBO.CreateAdmin(admin);
                }

                return new JsonResult(msg);
            }
        }

        //禁启用账号
        public ActionResult<Message> UpdateAdminState()
        {
            string[] idsStr = Request.Form["ids"];
            string stateStr = Request.Form["state"];
            byte state = 1;
            if (Validator.IsNumbers(stateStr))
            {
                state = byte.Parse(stateStr);
            }

            var stateDes = state == 1 ? "启用" : "禁用";

            var msg = new Message(10, $"{stateDes}失败");
            var idsInt = new List<int>();

            if (idsStr != null && idsStr.Count() > 0)
            {
                foreach (var id in idsStr)
                {
                    if (Validator.IsNumbers(id))
                    {
                        idsInt.Add(int.Parse(id));
                    }
                }

                msg = CMSAdminBO.UpdateStateByIDs(idsInt, state);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = $"请选择要{stateDes}的账号";
            }

            return new JsonResult(msg);
        }

        //解锁账号
        public ActionResult<Message> UnlockAdmin()
        {
            string[] idsStr = Request.Form["ids"];

            var msg = new Message(10, "解锁失败");
            var idsInt = new List<int>();

            if (idsStr != null && idsStr.Count() > 0)
            {
                foreach (var id in idsStr)
                {
                    if (Validator.IsNumbers(id))
                    {
                        idsInt.Add(int.Parse(id));
                    }
                }

                msg = CMSAdminBO.UnlockByIDs(idsInt);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = "请选择要解锁的账号";
            }

            return new JsonResult(msg);
        }

        //删除账号
        public ActionResult<Message> DeleteAdmin()
        {
            string[] idsStr = Request.Form["ids"];

            var msg = new Message(10, "删除失败");
            var idsInt = new List<int>();

            if (idsStr != null && idsStr.Count() > 0)
            {
                foreach (var id in idsStr)
                {
                    if (Validator.IsNumbers(id))
                    {
                        idsInt.Add(int.Parse(id));
                    }
                }

                msg = CMSAdminBO.DeleteAdminByIDs(idsInt);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = "请选择要删除的账号";
            }

            return new JsonResult(msg);
        }

        #endregion
    }
}