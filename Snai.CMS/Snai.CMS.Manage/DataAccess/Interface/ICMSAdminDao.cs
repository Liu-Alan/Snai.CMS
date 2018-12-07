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

        //取某id管理员
        Admin GetAdminByID(int id);

        //取管理员
        Admin GetAdminByUserName(string userName);

        //根据id更新管理员
        bool UpdateAdminByID(int id, string userName,string password, byte state,int roleID);

        //修改密码
        bool UpdatePasswordByID(int id, string password);

        //更新状态
        bool UpdateStateByID(int id, byte state);

        //解锁
        bool UnlockByID(int id,int lockMinute);

        //根据id删掉管理员
        bool DeleteAdminByID(int id);

        //根据id删掉管理员
        bool DeleteAdminByIDs(IEnumerable<int> ids);
    }
}
