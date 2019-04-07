using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models.BackManage
{
    public class ModuleListModel: LayoutModel
    {
        public ModuleListModel()
        {
            Modules = new List<Module>();
        }

        public IList<Module> Modules { get; set; }
    }
}
