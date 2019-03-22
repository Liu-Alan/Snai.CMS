using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Models
{
    public class LayoutModel
    {
        public LayoutModel()
        {
            RoleModules = new List<Module>();
            ThisModules = new List<int>();
        }
        //页面标题
        public string PageTitle { get; set; }

        //网站标题
        public string WebTitle { get; set; }

        //用户名
        public string UserName { get; set; }

        //角色名
        public string RoleTitle { get; set; }

        //菜单
        public IList<Module> RoleModules { get; set; }

        //当前菜单
        public IList<int> ThisModules { get; set; }

        #region 公有方法

        //父类转子类
        public void ToT<T>(ref T t) where T: LayoutModel
        {
            t.PageTitle = this.PageTitle;
            t.WebTitle = this.WebTitle;
            t.UserName = this.UserName;
            t.RoleTitle = this.RoleTitle;
            t.RoleModules = this.RoleModules;
            t.ThisModules = this.ThisModules;
        }

        #endregion
    }
}
