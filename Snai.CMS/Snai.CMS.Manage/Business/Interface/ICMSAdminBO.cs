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

        //更新错误登录信息
        Message UpdateErrorLogon(int id, int errorLogonTime, int errorLogonCount);

        //锁定管理员
        Message LockAdmin(int id, int lockTime);

        //解锁
        Message UnlockByIDs(IEnumerable<int> ids);

        //删除管理员
        Message DeleteAdminByIDs(IEnumerable<int> ids);

        //更新管理员登录信息
        Message UpdateAdminLogon(int id, int lastLogonTime, string lastLogonIP);

        #endregion

        #region 管理员登录

        //登录
        Message AdminLogin(AdminLogin admin);

        //登出
        void AdminLogout();

        //是否登录（Message.Success true 登录在线，false 离线）
        Message VerifyAdminLogin();

        #endregion

        #region 菜单

        //取菜单
        Module GetModule(string controller, string action);

        //取菜单
        IEnumerable<Module> GetModulesByIDs(IEnumerable<int> ids);

        #endregion

        #region 角色

        //取角色
        Role GetRoleByID(int id);

        #endregion

        #region 权限

        //取权限
        RoleRight GetRoleRight(int roleID, int moduleID);

        //取权限
        IEnumerable<RoleRight> GetRoleRights(int roleID);

        #endregion

        #region 用户权限

        //权限判断（Message.Success true 权限成功，false 权限失败）
        Message VerifyUserRole(string UserName, string controller, string action);

        //取角色下菜单
        IEnumerable<Module> GetModulesByRoleID(int roleID);

        #endregion
    }
}
