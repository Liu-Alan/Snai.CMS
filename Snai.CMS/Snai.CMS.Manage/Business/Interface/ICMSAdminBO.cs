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
        #region 账号操作

        //添加账号
        Message CreateAdmin(Admin admin);

        //取全部账号
        IEnumerable<Admin> GetAdmins();

        //取账号
        Admin GetAdminByID(int id);

        //取账号
        Admin GetAdminByUserName(string userName);

        //取账号
        IEnumerable<Admin> GetAdmins(string userName, int roleID, int pageLimit, int pageIndex);

        //取账号数
        int GetAdminCount(string userName, int roleID);

        //更新账号
        Message UpdateAdminByID(Admin admin);

        //修改密码
        Message UpdatePasswordByID(int id,string oldPassword, string password,string rePassword);

        //更新状态
        Message UpdateAdminStateByIDs(IEnumerable<int> ids, byte state);

        //更新错误登录信息
        Message UpdateErrorLogon(int id, int errorLogonTime, int errorLogonCount);

        //锁定账号
        Message LockAdmin(int id, int lockTime);

        //解锁
        Message UnlockByIDs(IEnumerable<int> ids);

        //删除账号
        Message DeleteAdminByIDs(IEnumerable<int> ids);

        //更新账号登录信息
        Message UpdateAdminLogon(int id, int lastLogonTime, string lastLogonIP);

        #endregion

        #region 账号登录

        //登录
        Message AdminLogin(AdminLogin admin);

        //登出
        void AdminLogout();

        //是否登录（Message.Success true 登录在线，false 离线）
        Message VerifyAdminLogin();

        #endregion

        #region 菜单

        //添加菜单
        Message CreateModule(Module module);

        //更新菜单
        Message UpdateModule(Module module);

        //取菜单
        Module GetModule(int id);

        //取菜单
        Module GetModule(string controller, string action);

        //取菜单
        IEnumerable<Module> GetModulesByIDs(IEnumerable<int> ids, int state);

        //取全部菜单
        IEnumerable<Module> GetModules(byte state);

        //取菜单
        IEnumerable<Module> GetModules(string title, int parentID, int pageLimit, int pageIndex);

        //取菜单
        IEnumerable<Module> GetModulesByParentID(int parentID, byte getSub);

        //取菜单数
        int GetModuleCount(string title, int parentID);

        //更新状态
        Message UpdateModuleState(IEnumerable<int> ids, byte state);

        //删除菜单
        Message DeleteModule(IEnumerable<int> ids);

        #endregion

        #region 角色

        //添加角色
        Message CreateRole(Role role);

        //更新角色
        Message UpdateRole(Role role);

        //取角色
        Role GetRoleByID(int id);

        //取全部角色
        IEnumerable<Role> GetRoles(byte state);

        //取角色
        IEnumerable<Role> GetRoles(string title, int pageLimit, int pageIndex);

        //取角色数
        int GetRoleCount(string title);

        //更新状态
        Message UpdateRoleState(IEnumerable<int> ids, byte state);

        //删除角色
        Message DeleteRole(IEnumerable<int> ids);

        #endregion

        #region 权限

        //添加权限
        Message CreateRoleRight(int roleID, IEnumerable<int> moduleIDs);

        //取权限
        RoleRight GetRoleRight(int roleID, int moduleID);

        //取权限
        IEnumerable<RoleRight> GetRoleRights(int roleID);

        //删除权限
        Message DeleteRoleRight(int roleID);

        //权限判断（Message.Success true 权限成功，false 权限失败）
        Message VerifyUserRole(string UserName, string controller, string action);

        //取角色下菜单
        IEnumerable<Module> GetModulesByRoleID(int roleID);

        //取当前菜单和父类菜单
        IEnumerable<int> GetThisModuleIDs(IEnumerable<Module> modules,int moduleID);

        #endregion
    }
}
