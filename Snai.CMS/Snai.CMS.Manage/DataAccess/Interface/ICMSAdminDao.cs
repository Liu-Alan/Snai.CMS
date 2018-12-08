using Snai.CMS.Manage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.DataAccess.Interface
{
    public interface ICMSAdminDao
    {
        //添加管理员
        bool CreateAdmin(Admin admin);

        //取全部管理员
        IEnumerable<Admin> GetAdmins();

        //取管理员
        Admin GetAdminByID(int id);

        //取管理员
        Admin GetAdminByUserName(string userName);

        //更新管理员
        bool UpdateAdminByID(int id, string userName,string password, byte state,int roleID);

        //修改密码
        bool UpdatePasswordByID(int id, string password);

        //更新状态
        bool UpdateStateByIDs(IEnumerable<int> ids, byte state);

        //解锁
        bool UnlockByIDs(IEnumerable<int> ids, int lockTime);

        //删掉管理员
        bool DeleteAdminByIDs(IEnumerable<int> ids);
    }
}
