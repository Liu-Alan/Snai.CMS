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
                msg.Code = 101;
                msg.Msg = "管理员不能为空";

                return msg;
            }

            if (string.IsNullOrEmpty(admin.UserName.Trim()))
            {
                msg.Code = 102;
                msg.Msg = "用户名不能为空";

                return msg;
            }

            var uAdmin = CMSAdminDao.GetAdminByUserName(admin.UserName);
            if (uAdmin != null && uAdmin.ID > 0)
            {
                msg.Code = 11;
                msg.Msg = "添加的管理员已存在";

                return msg;
            }

            if (string.IsNullOrEmpty(admin.Password.Trim()) || !admin.Password.Trim().Equals(admin.RePassword))
            {
                msg.Code = 103;
                msg.Msg = "密码为空或两次密码不一致";

                return msg;
            }

            var valPwd = Validator.IsPassword(admin.Password);
            if (valPwd == 1)
            {
                msg.Code = 105;
                msg.Msg = "密码包含空格";

                return msg;
            }
            if (valPwd == 2)
            {
                msg.Code = 106;
                msg.Msg = "密码长度不足";

                return msg;
            }
            if (valPwd == 4)
            {
                msg.Code = 107;
                msg.Msg = "密码不能为空";

                return msg;
            }
            if (valPwd == 5)
            {
                msg.Code = 108;
                msg.Msg = "密码不能是同一个字符";

                return msg;
            }
            if (valPwd == 6)
            {
                msg.Code = 109;
                msg.Msg = "密码不能是递增或递减的数字或字母";

                return msg;
            }
            if (valPwd == 7)
            {
                msg.Code = 110;
                msg.Msg = "密码属于社会工程学中字符";

                return msg;
            }
            if (valPwd == 8)
            {
                msg.Code = 111;
                msg.Msg = "密码必须是字母与(数字或特殊符号)组合";

                return msg;
            }

            if (admin.RoleID <= 0)
            {
                msg.Code = 104;
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
