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
                //取账号列表
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
                byte state = 1;
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

                msg = CMSAdminBO.UpdateAdminStateByIDs(idsInt, state);
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

        #region 菜单管理

        //菜单管理
        public IActionResult ModuleList(string id)
        {
            if (id == null || !id.ToUpper().Equals("DATA", StringComparison.OrdinalIgnoreCase))
            {
                // 权限和菜单
                ModuleListModel model = new ModuleListModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                var modules = CMSAdminBO.GetModules(0);
                if (modules != null)
                {
                    model.Modules = modules.ToList();
                }

                return View(model);
            }
            else
            {
                //取菜单列表
                string titleFilter = Request.Query["title"];

                int parentIDFilter = 0;
                int.TryParse(Request.Query["parentID"], out parentIDFilter);

                int pageIndex = 0;
                int.TryParse(Request.Query["page"], out pageIndex);

                int pageLimit = Consts.Page_Limit;
                int totCount = CMSAdminBO.GetModuleCount(titleFilter, parentIDFilter);
                int pageCount = (int)Math.Ceiling(totCount / (float)pageLimit);
                var modules = new List<Module>();
                if (totCount > 0)
                {
                    IEnumerable<Module> moduleIE = CMSAdminBO.GetModules(titleFilter, parentIDFilter, pageLimit, pageIndex);
                    if (moduleIE != null)
                    {
                        modules = moduleIE.ToList();
                    }
                }

                dynamic model = new ExpandoObject();

                model.code = 0;
                model.msg = "";
                model.count = totCount;
                model.data = modules.Select(s => new
                {
                    id = s.ID,
                    title = s.Title,
                    parentTitle = s.ParentTitle,
                    controller = s.Controller,
                    action = s.Action,
                    state = s.State
                });

                return new JsonResult(model);
            }
        }

        //添加修改菜单
        public IActionResult ModifyModule()
        {
            //展示页面
            if (!Request.Method.ToUpper().Equals("POST", StringComparison.OrdinalIgnoreCase) || !Request.HasFormContentType)
            {
                // 权限和菜单
                ModifyModuleModel model = new ModifyModuleModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                var modules = CMSAdminBO.GetModules(0);
                if (modules != null)
                {
                    model.Modules = modules.ToList();
                }

                int id = 0;
                int.TryParse(Request.Query["id"], out id);

                if (id > 0)
                {
                    model.PageTitle = "修改菜单";
                    var module = CMSAdminBO.GetModule(id);
                    if (module != null && module.ID > 0)
                    {
                        model.Module = module;
                    }
                }
                else
                {
                    model.PageTitle = "添加菜单";
                }

                return View(model);
            }
            else
            {
                var msg = new Message(10, "修改失败！");

                int id = 0;
                int.TryParse(Request.Form["id"], out id);
                int parentID = 0;
                int.TryParse(Request.Form["parentID"], out parentID);
                string title = Request.Form["title"];
                string controller = Request.Form["controller"];
                string action = Request.Form["action"];
                int sort = 0;
                int.TryParse(Request.Form["sort"], out sort);
                byte state = 1;
                byte.TryParse(Request.Form["state"], out state);

                var module = new Module()
                {
                    ID = id,
                    ParentID = parentID,
                    Title = title,
                    Controller = controller,
                    Action = action,
                    Sort = sort,
                    State = state

                };

                if (module.ID > 0)
                {
                    msg = CMSAdminBO.UpdateModule(module);
                }
                else
                {
                    msg = CMSAdminBO.CreateModule(module);
                }

                return new JsonResult(msg);
            }
        }

        //禁启用菜单
        public ActionResult<Message> UpdateModuleState()
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

                msg = CMSAdminBO.UpdateModuleState(idsInt, state);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = $"请选择要{stateDes}的菜单";
            }

            return new JsonResult(msg);
        }

        //删除菜单
        public ActionResult<Message> DeleteModule()
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

                msg = CMSAdminBO.DeleteModule(idsInt);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = "请选择要删除的菜单";
            }

            return new JsonResult(msg);
        }

        #endregion

        #region 角色权限管理

        //角色管理
        public IActionResult RoleList(string id)
        {
            if (id == null || !id.ToUpper().Equals("DATA", StringComparison.OrdinalIgnoreCase))
            {
                // 权限和菜单
                RoleListModel model = new RoleListModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                return View(model);
            }
            else
            {
                //取角色列表
                string titleFilter = Request.Query["title"];

                int pageIndex = 0;
                int.TryParse(Request.Query["page"], out pageIndex);

                int pageLimit = Consts.Page_Limit;
                int totCount = CMSAdminBO.GetRoleCount(titleFilter);
                int pageCount = (int)Math.Ceiling(totCount / (float)pageLimit);
                var roles = new List<Role>();
                if (totCount > 0)
                {
                    IEnumerable<Role> roleIE = CMSAdminBO.GetRoles(titleFilter, pageLimit, pageIndex);
                    if (roleIE != null)
                    {
                        roles = roleIE.ToList();
                    }
                }

                dynamic model = new ExpandoObject();

                model.code = 0;
                model.msg = "";
                model.count = totCount;
                model.data = roles.Select(s => new
                {
                    id = s.ID,
                    title = s.Title,
                    state = s.State
                });

                return new JsonResult(model);
            }

        }

        //添加修改角色
        public IActionResult ModifyRole()
        {
            //展示页面
            if (!Request.Method.ToUpper().Equals("POST", StringComparison.OrdinalIgnoreCase) || !Request.HasFormContentType)
            {
                // 权限和菜单
                ModifyRoleModel model = new ModifyRoleModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                int id = 0;
                int.TryParse(Request.Query["id"], out id);

                if (id > 0)
                {
                    model.PageTitle = "修改角色";
                    var role = CMSAdminBO.GetRoleByID(id);
                    if (role != null && role.ID > 0)
                    {
                        model.Role = role;
                    }
                }
                else
                {
                    model.PageTitle = "添加角色";
                }

                return View(model);
            }
            else
            {
                var msg = new Message(10, "修改失败！");

                int id = 0;
                int.TryParse(Request.Form["id"], out id);
                string title = Request.Form["title"];
                byte state = 1;
                byte.TryParse(Request.Form["state"], out state);

                var role = new Role()
                {
                    ID = id,
                    Title = title,
                    State = state

                };

                if (role.ID > 0)
                {
                    msg = CMSAdminBO.UpdateRole(role);
                }
                else
                {
                    msg = CMSAdminBO.CreateRole(role);
                }

                return new JsonResult(msg);
            }
        }

        //禁启用角色
        public ActionResult<Message> UpdateRoleState()
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

                msg = CMSAdminBO.UpdateRoleState(idsInt, state);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = $"请选择要{stateDes}的角色";
            }

            return new JsonResult(msg);
        }

        //删除角色
        public ActionResult<Message> DeleteRole()
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

                msg = CMSAdminBO.DeleteRole(idsInt);
            }
            else
            {
                msg.Code = 101;
                msg.Msg = "请选择要删除的角色";
            }

            return new JsonResult(msg);
        }

        //添加权限
        public IActionResult ModifyRoleRight()
        {
            //展示页面
            if (!Request.Method.ToUpper().Equals("POST", StringComparison.OrdinalIgnoreCase) || !Request.HasFormContentType)
            {
                // 权限和菜单
                ModifyRoleRightModel model = new ModifyRoleRightModel();
                var layoutModel = this.GetLayoutModel();
                if (layoutModel != null)
                {
                    layoutModel.ToT(ref model);
                }

                int id = 0;
                int.TryParse(Request.Query["id"], out id);

                if (id > 0)
                {
                    var role = CMSAdminBO.GetRoleByID(id);
                    if (role != null && role.ID > 0)
                    {
                        model.Role = role;

                        var modules = CMSAdminBO.GetModules(1);
                        if (modules != null)
                        {
                            model.Modules = modules.ToList();
                        }

                        var roleRights = CMSAdminBO.GetRoleRights(role.ID);
                        if (roleRights != null)
                        {
                            model.RoleModuleIDs = roleRights.Select(s => s.ModuleID).ToList();
                        }
                    }
                }

                return View(model);
            }
            else
            {
                var msg = new Message(10, "分配失败！");

                int id = 0;
                int.TryParse(Request.Form["id"], out id);
                string title = Request.Form["title"];
                byte state = 1;
                byte.TryParse(Request.Form["state"], out state);

                var role = new Role()
                {
                    ID = id,
                    Title = title,
                    State = state

                };

                if (role.ID > 0)
                {
                    msg = CMSAdminBO.UpdateRole(role);
                }
                else
                {
                    msg = CMSAdminBO.CreateRole(role);
                }

                return new JsonResult(msg);
            }
        }

        #endregion
    }
}