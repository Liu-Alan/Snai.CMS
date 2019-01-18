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

        #region 管理员操作

        //添加管理员
        public bool CreateAdmin(Admin admin)
        {
            Context.Admins.Add(admin);
            return Context.SaveChanges() > 0;
        }

        //取全部管理员
        public IEnumerable<Admin> GetAdmins()
        {
            return Context.Admins.ToList();
        }

        //取管理员
        public Admin GetAdminByID(int id)
        {
            return Context.Admins.SingleOrDefault(s => s.ID == id);
        }

        //取管理员
        public Admin GetAdminByUserName(string userName)
        {
            return Context.Admins.SingleOrDefault(s => s.UserName == userName);
        }

        //更新管理员
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
            var admins = Context.Admins.Where(item => ids.Contains(item.ID));
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

        //解锁
        public bool UnlockByIDs(IEnumerable<int> ids, int lockTime, int updateTime)
        {
            var upState = false;
            var admins = Context.Admins.Where(item => ids.Contains(item.ID));
            if (admins != null && admins.Count() > 0)
            {
                foreach (var admin in admins)
                {
                    admin.ErrorLogonCount = 0;
                    admin.LockTime = lockTime;
                    admin.UpdateTime = updateTime;
                }

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //删除管理员
        public bool DeleteAdminByIDs(IEnumerable<int> ids)
        {
            var admins = Context.Admins.Where(item => ids.Contains(item.ID));
            Context.Admins.RemoveRange(admins);
            return Context.SaveChanges() > 0;
        }

        #endregion
    }
}
