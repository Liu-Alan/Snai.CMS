using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models
{
    public class LayoutModel
    {
        //页面标题
        public string PageTitle { get; set; }

        //网站标题
        public string WebTitle { get; set; }

        //用户名
        public string UserName { get; set; }

        //角色名
        public string RoleTitle { get; set; }

        //菜单
        public IEnumerable<Module> RoleModules { get; set; }

        //当前菜单
        public IEnumerable<int> ThisModules { get; set; }
        
    }
}
