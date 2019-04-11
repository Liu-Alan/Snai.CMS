using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class ModifyRoleModel : LayoutModel
    {
        public ModifyRoleModel()
        {
            Role = new Role();
        }

        public Role Role { get; set; }
    }
}
