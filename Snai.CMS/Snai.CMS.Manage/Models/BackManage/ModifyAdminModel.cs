using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class ModifyAdminModel : LayoutModel
    {
        public ModifyAdminModel()
        {
            Roles = new List<Role>();
            Admin = new Admin();
        }

        public Admin Admin { get; set; }

        public IList<Role> Roles { get; set; }
    }
}
