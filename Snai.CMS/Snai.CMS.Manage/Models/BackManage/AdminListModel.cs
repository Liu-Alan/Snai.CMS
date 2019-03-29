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
            Roles = new List<Role>();
        }

        public IList<Role> Roles { get; set; }
    }
}
