using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class AdminListModel : LayoutModel
    {
        public AdminListModel()
        {
            Admins = new List<Admin>();
            Roles = new List<Role>();
        }

        //管理员列表
        public IList<Admin> Admins { get; set; }

        //权限列表
        public IList<Role> Roles { get; set; }

        //过滤条件
        public string UserNameFilter { get; set; }
        public string RoleIDFilter { get; set; }
    }
}
