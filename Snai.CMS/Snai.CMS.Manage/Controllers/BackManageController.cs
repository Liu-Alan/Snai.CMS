using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Infrastructure.Extension;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Common.Infrastructure.ValidateCodes;
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

        #region 管理员管理

        public IActionResult AdminList()
        {
            // 权限和菜单
            AdminListModel model = new AdminListModel();
            var layoutModel = this.GetLayoutModel();
            if (layoutModel != null)
            {
                layoutModel.ToT(ref model);
            }

            //取管理员列表分布
            model.UserNameFilter = Request.Query["userName"];

            var roleID = 0;
            int.TryParse( Request.Query["roleID"], out roleID);
            model.RoleIDFilter = roleID;

            var pageIndex = 0;
            int.TryParse(Request.Query["pageIndex"], out pageIndex);
            model.PageIndex = pageIndex;

            model.PageLimit = Consts.Page_Limit;
            model.TotCount = CMSAdminBO.GetAdminCount(model.UserNameFilter, model.RoleIDFilter);
            model.PageCount = (int)Math.Ceiling(model.TotCount / (float)model.PageLimit);

            if (model.TotCount > 0)
            {
                var admins = CMSAdminBO.GetAdmins(model.UserNameFilter, model.RoleIDFilter, model.PageLimit, model.PageIndex);
                if (admins != null)
                {
                    model.Admins = admins.ToList();
                }
            }

            var roles = CMSAdminBO.GetRoles(1);
            if (roles != null)
            {
                model.Roles = roles.ToList();
            }

            return View(model);
        }

        #endregion
    }
}