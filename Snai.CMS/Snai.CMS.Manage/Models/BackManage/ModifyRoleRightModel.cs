using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class ModifyRoleRightModel : LayoutModel
    {
        public ModifyRoleRightModel()
        {
            Role = new Role();
            RoleModuleIDs = new List<int>();
            Modules = new List<Module>();
        }

        public Role Role { get; set; }

        public IList<int> RoleModuleIDs { get; set; }

        public IList<Module> Modules { get; set; }
    }
}
