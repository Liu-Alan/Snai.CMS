using Snai.CMS.Manage.DataAccess.Base;
using Snai.CMS.Manage.DataAccess.Interface;
using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.DataAccess.Implement
{
    public class CMSAdminDao : ICMSAdminDao
    {
        #region 属性声明

        public CMSContext Context;

        #endregion

        #region 构造函数

        public CMSAdminDao(CMSContext context)
        {
            Context = context;
        }

        #endregion

        #region 账号操作

        //添加账号
        public bool CreateAdmin(Admin admin)
        {
            Context.Admins.Add(admin);
            return Context.SaveChanges() > 0;
        }

        //取全部账号
        public IEnumerable<Admin> GetAdmins()
        {
            return Context.Admins;
        }

        //取账号
        public Admin GetAdminByID(int id)
        {
            return Context.Admins.SingleOrDefault(s => s.ID == id);
        }

        //取账号
        public Admin GetAdminByUserName(string userName)
        {
            return Context.Admins.SingleOrDefault(s => s.UserName == userName);
        }

        //取账号
        public IEnumerable<Admin> GetAdminsLikeUserName(string userName)
        {
            return Context.Admins.Where(s => s.UserName.Contains(userName));
        }

        //取账号
        public IEnumerable<Admin> GetAdminsByRoleID(int roleID)
        {
            return Context.Admins.Where(s => s.RoleID == roleID);
        }

        //更新账号
        public bool UpdateAdminByID(int id, string userName, string password, byte state, int roleID, int updateTime)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.UserName = userName;
                if (!string.IsNullOrEmpty(password.Trim()))
                {
                    admin.Password = password;
                }
                admin.State = state;
                admin.RoleID = roleID;
                admin.UpdateTime = updateTime;

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //修改密码
        public bool UpdatePasswordByID(int id, string password, int updateTime)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.Password = password;
                admin.UpdateTime = updateTime;

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //更新状态
        public bool UpdateStateByIDs(IEnumerable<int> ids, byte state, int updateTime)
        {
            var upState = false;
            var admins = Context.Admins.Where(s => ids.Contains(s.ID));
            if (admins != null && admins.Count() > 0)
            {
                foreach (var admin in admins)
                {
                    admin.State = state;
                    admin.UpdateTime = updateTime;
                }

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //更新错误登录信息
        public bool UpdateErrorLogon(int id, int errorLogonTime, int errorLogonCount, int updateTime)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.ErrorLogonTime = errorLogonTime;
                admin.ErrorLogonCount = errorLogonCount;
                admin.UpdateTime = updateTime;

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //锁定账号
        public bool LockAdmin(int id, int lockTime, int updateTime)
        {
            var upState = false;

            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.ErrorLogonTime = 0;
                admin.ErrorLogonCount = 0;
                admin.LockTime = lockTime;
                admin.UpdateTime = updateTime;

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //解锁
        public bool UnlockAdminByIDs(IEnumerable<int> ids, int updateTime)
        {
            var upState = false;
            var admins = Context.Admins.Where(s => ids.Contains(s.ID));
            if (admins != null && admins.Count() > 0)
            {
                foreach (var admin in admins)
                {
                    admin.ErrorLogonTime = 0;
                    admin.ErrorLogonCount = 0;
                    admin.LockTime = 0;
                    admin.UpdateTime = updateTime;
                }

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //删除账号
        public bool DeleteAdminByIDs(IEnumerable<int> ids)
        {
            var admins = Context.Admins.Where(s => ids.Contains(s.ID));
            Context.Admins.RemoveRange(admins);
            return Context.SaveChanges() > 0;
        }


        //更新账号登录信息
        public bool UpdateAdminLogon(int id, int lastLogonTime, string lastLogonIP)
        {
            var upState = false;

            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.LastLogonTime = lastLogonTime;
                admin.ErrorLogonTime = 0;
                admin.ErrorLogonCount = 0;
                admin.LockTime = 0;
                admin.LastLogonIP = lastLogonIP;

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        #endregion

        #region 菜单

        //取菜单
        public Module GetModule(int id)
        {
            return Context.Modules.SingleOrDefault(s => s.ID == id);
        }

        //取菜单
        public Module GetModule(string controller, string action)
        {
            return Context.Modules.FirstOrDefault(s => s.Controller == controller && s.Action == action);
        }

        //取菜单
        public IEnumerable<Module> GetModulesByIDs(IEnumerable<int> ids, int state)
        {
            if (ids == null)
            {
                return null;
            }

            var idList = ids.ToList();

            if (state == 0)
            {
                return Context.Modules.Where(s => ids.Contains(s.ID));
            }
            else
            {
                var modules = Context.Modules.Where(s => idList.Contains(s.ID) && s.State == state);
                return modules;
            }
        }

        //取全部菜单
        public IEnumerable<Module> GetModules(byte state)
        {
            if (state > 0)
            {
                return Context.Modules.Where(s => s.State == state);
            }
            else
            {
                return Context.Modules;
            }
        }

        //取菜单
        public IEnumerable<Module> GetModulesLikeTitle(string title)
        {
            return Context.Modules.Where(s => s.Title.Contains(title));
        }

        //取菜单
        public IEnumerable<Module> GetModulesByParentID(int parentID)
        {
            return Context.Modules.Where(s => s.ParentID == parentID);
        }

        #endregion

        #region 角色

        //取角色
        public Role GetRoleByID(int id)
        {
            return Context.Roles.SingleOrDefault(s => s.ID == id);
        }

        //取全部角色
        public IEnumerable<Role> GetRoles(byte state)
        {
            if (state > 0)
            {
                return Context.Roles.Where(s => s.State == state);
            }
            else
            {
                return Context.Roles;
            }
        }

        #endregion

        #region 权限

        //取权限
        public RoleRight GetRoleRight(int roleID, int moduleID)
        {
            return Context.RoleRights.SingleOrDefault(s => s.RoleID == roleID && s.ModuleID == moduleID);
        }

        //取权限
        public IEnumerable<RoleRight> GetRoleRights(int roleID)
        {
            return Context.RoleRights.Where(s => s.RoleID == roleID);
        }

        #endregion
    }
}
