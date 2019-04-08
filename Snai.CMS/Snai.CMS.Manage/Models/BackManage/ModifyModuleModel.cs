using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class ModifyModuleModel : LayoutModel
    {
        public ModifyModuleModel()
        {
            Module = new Module();
            Modules = new List<Module>();
        }

        public Module Module { get; set; }

        public IList<Module> Modules { get; set; }
    }
}
