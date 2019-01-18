using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Business.Interface
{
    public interface ICMSAdminBO
    {
        #region 管理员操作

        //添加管理员
        Message CreateAdmin(Admin admin);

        //取全部管理员
        IEnumerable<Admin> GetAdmins();

        //取管理员
        Admin GetAdminByID(int id);

        //取管理员
        Admin GetAdminByUserName(string userName);

        //更新管理员
        Message UpdateAdminByID(Admin admin);

        //修改密码
        Message UpdatePasswordByID(int id,string oldPassword, string password,string rePassword);

        //更新状态
        Message UpdateStateByIDs(IEnumerable<int> ids, byte state);

        //解锁
        Message UnlockByIDs(IEnumerable<int> ids);

        //删除管理员
        Message DeleteAdminByIDs(IEnumerable<int> ids);

        #endregion
    }
}
