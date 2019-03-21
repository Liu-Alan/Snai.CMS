using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.DataAccess.Interface
{
    public interface ICMSAdminDao
    {
        #region 管理员操作

        //添加管理员
        bool CreateAdmin(Admin admin);

        //取全部管理员
        IEnumerable<Admin> GetAdmins();

        //取管理员
        Admin GetAdminByID(int id);

        //取管理员
        Admin GetAdminByUserName(string userName);

        //取管理员
        IEnumerable<Admin> GetAdminsLikeUserName(string userName);

        //取管理员
        IEnumerable<Admin> GetAdminsByRoleID(int roleID);

        //更新管理员
        bool UpdateAdminByID(int id, string userName, string password, byte state, int roleID, int updateTime);

        //修改密码
        bool UpdatePasswordByID(int id, string password, int updateTime);

        //更新状态
        bool UpdateStateByIDs(IEnumerable<int> ids, byte state, int updateTime);

        //更新错误登录信息
        bool UpdateErrorLogon(int id, int errorLogonTime,int errorLogonCount, int updateTime);

        //锁定管理员
        bool LockAdmin(int id, int lockTime, int updateTime);

        //解锁
        bool UnlockAdminByIDs(IEnumerable<int> ids, int updateTime);

        //删除管理员
        bool DeleteAdminByIDs(IEnumerable<int> ids);

        //更新管理员登录信息
        bool UpdateAdminLogon(int id, int lastLogonTime, string lastLogonIP);

        #endregion

        #region 菜单

        //取菜单
        Module GetModule(string controller, string action);

        //取菜单
        IEnumerable<Module> GetModulesByIDs(IEnumerable<int> ids, int state);

        #endregion

        #region 角色

        //取角色
        Role GetRoleByID(int id);

        //取全部角色
        IEnumerable<Role> GetRoles(byte state);

        #endregion

        #region 权限

        //取权限
        RoleRight GetRoleRight(int roleID, int moduleID);

        //取权限
        IEnumerable<RoleRight> GetRoleRights(int roleID);

        #endregion
    }
}
