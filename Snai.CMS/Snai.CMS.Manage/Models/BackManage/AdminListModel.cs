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
        }

        //管理员列表
        public IList<Admin> Admins { get; set; }
    }
}
