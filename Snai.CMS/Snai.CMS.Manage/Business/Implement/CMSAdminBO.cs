using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.DataAccess.Interface;
using Snai.CMS.Manage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Business.Implement
{
    public class CMSAdminBO
    {
        public ICMSAdminDao CMSAdminDao;

        public CMSAdminBO(ICMSAdminDao cmsAdminDao)
        {
            CMSAdminDao = cmsAdminDao;
        }

        //添加管理员
        public Message CreateAdmin(Admin admin)
        {
            var msg = new Message(10,"");
            if (admin == null)
            {
                msg.Code = 10;
                msg.Msg = "管理员不能为空";

                return msg;
            }

            if (string.IsNullOrEmpty(admin.UserName.Trim()))
            {
                msg.Code = 11;
                msg.Msg = "用户名不能为空";

                return msg;
            }

            var uAdmin = CMSAdminDao.GetAdminByUserName(admin.UserName);
            if (uAdmin != null && uAdmin.ID > 0)
            {
                msg.Code = 20;
                msg.Msg = "添加的管理员已存在";

                return msg;
            }

            if (string.IsNullOrEmpty(admin.Password.Trim()) || !admin.Password.Trim().Equals(admin.RePassword))
            {
                msg.Code = 12;
                msg.Msg = "密码为空或两次密码不一致";

                return msg;
            }

            if (admin.RoleID <= 0)
            {
                msg.Code = 13;
                msg.Msg = "清选择用户的角色";

                return msg;
            }

            admin.Password = EncryptMd5.EncryptByte(admin.Password.Trim());
            admin.CreateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var addState = CMSAdminDao.CreateAdmin(admin);

            if (addState)
            {
                msg.Code = 0;
                msg.Msg = "添加管理员成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "添加管理员失败";
            }

            return msg;
        }
    }
}
