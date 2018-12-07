using Snai.CMS.Manage.DataAccess.Base;
using Snai.CMS.Manage.DataAccess.Interface;
using Snai.CMS.Manage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.DataAccess.Implement
{
    public class CMSAdminDao : ICMSAdminDao
    {
        public CMSContext Context;

        public CMSAdminDao(CMSContext context)
        {
            Context = context;
        }

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

        //取某id管理员
        public Admin GetAdminByID(int id)
        {
            return Context.Admins.SingleOrDefault(s => s.ID == id);
        }

        //取管理员
        public Admin GetAdminByUserName(string userName)
        {
            return Context.Admins.SingleOrDefault(s => s.UserName == userName);
        }

        //根据id更新管理员
        public bool UpdateAdminByID(int id, string userName, string password, byte state, int roleID)
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

                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //修改密码
        public bool UpdatePasswordByID(int id, string password)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.Password = password;
                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //更新状态
        public bool UpdateStateByID(int id, byte state)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.State = state;
                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //解锁
        public bool UnlockByID(int id, int lockMinute)
        {
            var upState = false;
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);

            if (admin != null)
            {
                admin.ErrorLogonCount = 0;




                upState = Context.SaveChanges() > 0;
            }

            return upState;
        }

        //根据id删掉管理员
        public bool DeleteAdminByID(int id)
        {
            var admin = Context.Admins.SingleOrDefault(s => s.ID == id);
            Context.Admins.Remove(admin);
            return Context.SaveChanges() > 0;
        }

        //根据id删掉管理员
        public bool DeleteAdminByIDs(IEnumerable<int> ids)
        {
            var admins = Context.Admins.Where(item => ids.Contains(item.ID));
            Context.Admins.RemoveRange(admins);
            return Context.SaveChanges() > 0;
        }
    }
}
