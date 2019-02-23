using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Encrypt;
using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Common.Infrastructure.HttpContexts;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.DataAccess.Interface;
using Snai.CMS.Manage.Entities.BackManage;
using Snai.CMS.Manage.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Business.Implement
{
    public class CMSAdminBO: ICMSAdminBO
    {
        #region 属性声明

        IOptions<LogonSettings> LogonSettings;
        public ICMSAdminDao CMSAdminDao;
        public IHttpSession HttpSession;
        public ICMSAdminCookie CMSAdminCookie;

        #endregion

        #region 构造函数

        public CMSAdminBO(IOptions<LogonSettings> logonSettings, ICMSAdminDao cmsAdminDao, IHttpSession httpSession, ICMSAdminCookie cmsAdminCookie)
        {
            LogonSettings = logonSettings;
            CMSAdminDao = cmsAdminDao;
            HttpSession = httpSession;
            CMSAdminCookie = cmsAdminCookie;
        }

        #endregion

        #region 管理员操作

        //验证密码是否合法
        public Message VerifyPassword(string password)
        {
            var msg = new Message(0, "");

            var valPwd = Validator.IsPassword(password);

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

            return msg;
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

            if (admin.UserName.Length > 32)
            {
                msg.Code = 102;
                msg.Msg = "用户名长度不能多于32个字符";

                return msg;
            }

            var uAdmin = CMSAdminDao.GetAdminByUserName(admin.UserName);
            if (uAdmin != null && uAdmin.ID > 0)
            {
                msg.Code = 11;
                msg.Msg = "添加的管理员用户名已存在";

                return msg;
            }

            if (string.IsNullOrEmpty(admin.Password.Trim()) || !admin.Password.Trim().Equals(admin.RePassword))
            {
                msg.Code = 103;
                msg.Msg = "密码为空或两次密码不一致";

                return msg;
            }

            var pwdMsg = this.VerifyPassword(admin.Password);
            if (!pwdMsg.Success)
            {
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
            admin.UpdateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

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

        //取全部管理员
        public IEnumerable<Admin> GetAdmins()
        {
            var admins = CMSAdminDao.GetAdmins();

            return admins;
        }

        //取管理员
        public Admin GetAdminByID(int id)
        {
            var admin = CMSAdminDao.GetAdminByID(id);

            if (admin != null)
            {
                return admin;
            }
            else
            {
                return null;
            }
        }

        //取管理员
        public Admin GetAdminByUserName(string userName)
        {
            var admin = CMSAdminDao.GetAdminByUserName(userName);

            if (admin != null)
            {
                return admin;
            }
            else
            {
                return null;
            }
        }

        //更新管理员
        public Message UpdateAdminByID(Admin admin)
        {
            var msg = new Message(10, "");

            if (string.IsNullOrEmpty(admin.UserName.Trim()))
            {
                msg.Code = 101;
                msg.Msg = "用户名不能为空";

                return msg;
            }

            if (admin.UserName.Length>32)
            {
                msg.Code = 101;
                msg.Msg = "用户名长度不能多于32个字符";

                return msg;
            }

            var upAdmin = this.GetAdminByID(admin.ID);
            if (upAdmin == null || upAdmin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "修改的管理员不存在";

                return msg;
            }

            upAdmin = this.GetAdminByUserName(admin.UserName);
            if (upAdmin != null && upAdmin.ID != admin.ID)
            {
                msg.Code = 12;
                msg.Msg = "修改的管理员用户名已存在";

                return msg;
            }

            if (!string.IsNullOrEmpty(admin.Password.Trim()))
            {
                if (!admin.Password.Trim().Equals(admin.RePassword))
                {
                    msg.Code = 102;
                    msg.Msg = "两次密码不一致";

                    return msg;
                }

                var pwdMsg = this.VerifyPassword(admin.Password);
                if (!pwdMsg.Success)
                {
                    return msg;
                }

                admin.Password = EncryptMd5.EncryptByte(admin.Password.Trim());
            }

            admin.UpdateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UpdateAdminByID(admin.ID, admin.UserName, admin.Password, admin.State, admin.RoleID, admin.UpdateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "修改管理员成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "修改管理员失败";
            }

            return msg;

        }

        //修改密码
        public Message UpdatePasswordByID(int id, string oldPassword, string password, string rePassword)
        {
            var msg = new Message(10, "");

            var admin = this.GetAdminByID(id);
            if (admin == null || admin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "修改的管理员不存在";

                return msg;
            }

            if (string.IsNullOrEmpty(oldPassword))
            {
                msg.Code = 101;
                msg.Msg = "旧密码不能为空";

                return msg;
            }

            oldPassword = EncryptMd5.EncryptByte(oldPassword);
            if (!oldPassword.Equals(admin.Password))
            {
                msg.Code = 12;
                msg.Msg = "旧密码输入错误";

                return msg;
            }

            if (string.IsNullOrEmpty(password.Trim()) || !password.Trim().Equals(rePassword))
            {
                msg.Code = 102;
                msg.Msg = "密码为空或两次密码不一致";

                return msg;
            }

            var pwdMsg = this.VerifyPassword(password);
            if (!pwdMsg.Success)
            {
                return msg;
            }

            password = EncryptMd5.EncryptByte(password.Trim());
            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UpdatePasswordByID(id, password, updateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "修改密码成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "修改密码失败";
            }

            return msg;
        }

        //更新状态
        public Message UpdateStateByIDs(IEnumerable<int> ids, byte state)
        {
            var msg = new Message(10, "");

            if (state != 1 && state != 2)
            {
                msg.Code = 101;
                msg.Msg = "要更改的状态有误";

                return msg;
            }

            var stateDes = state == 1 ? "启用" : "禁用";

            if (ids == null || ids.Count() <= 0)
            {
                msg.Code = 101;
                msg.Msg = $"请选择要{stateDes}的管理员";

                return msg;
            }

            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UpdateStateByIDs(ids, state, updateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = $"{stateDes}成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = $"{stateDes}失败";
            }

            return msg;
        }

        //更新错误登录信息
        public Message UpdateErrorLogon(int id, int errorLogonTime, int errorLogonCount)
        {
            var msg = new Message(10, "");

            var admin = this.GetAdminByID(id);
            if (admin == null || admin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "更新的管理员不存在";

                return msg;
            }

            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UpdateErrorLogon(id, errorLogonTime, errorLogonCount, updateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "更新错误登录信息成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "更新错误登录信息失败";
            }

            return msg;
        }

        //锁定管理员
        public Message LockAdmin(int id, int lockTime)
        {
            var msg = new Message(10, "");

            var admin = this.GetAdminByID(id);
            if (admin == null || admin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "锁定的管理员不存在";

                return msg;
            }

            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.LockAdmin(id, lockTime, updateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "锁定的管理员成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "锁定的管理员失败";
            }

            return msg;
        }

        //解锁
        public Message UnlockByIDs(IEnumerable<int> ids)
        {
            var msg = new Message(10, "");

            if (ids == null || ids.Count() <= 0)
            {
                msg.Code = 101;
                msg.Msg = "请选择要解锁的管理员";

                return msg;
            }

            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UnlockAdminByIDs(ids, updateTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "解锁成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "解锁失败";
            }

            return msg;
        }

        //删除管理员
        public Message DeleteAdminByIDs(IEnumerable<int> ids)
        {
            var msg = new Message(10, "");

            if (ids == null || ids.Count() <= 0)
            {
                msg.Code = 101;
                msg.Msg = "请选择要删除的管理员";

                return msg;
            }

            var upState = CMSAdminDao.DeleteAdminByIDs(ids);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "删除成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "删除失败";
            }

            return msg;
        }

        //更新管理员登录信息
        public Message UpdateAdminLogon(int id, int lastLogonTime)
        {
            var msg = new Message(10, "");

            var admin = this.GetAdminByID(id);
            if (admin == null || admin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "更新的管理员不存在";

                return msg;
            }

            var updateTime = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);

            var upState = CMSAdminDao.UpdateAdminLogon(id, lastLogonTime);

            if (upState)
            {
                msg.Code = 0;
                msg.Msg = "更新管理员登录信息成功";
            }
            else
            {
                msg.Code = 1;
                msg.Msg = "更新管理员登录信息失败";
            }

            return msg;
        }

        #endregion

        #region 管理员登录

        //登录
        public Message AdminLogin(AdminLogin adminLogin)
        {
            var msg = new Message(10, "");

            if (string.IsNullOrEmpty(adminLogin.UserName) || string.IsNullOrEmpty(adminLogin.Password))
            {
                msg.Code = 101;
                msg.Msg = "用户名或密码不能为空";

                return msg;
            }

            if (adminLogin.UserName.Length > 32)
            {
                msg.Code = 101;
                msg.Msg = "用户名或密码输入错误";

                return msg;
            }

            if (string.IsNullOrEmpty(adminLogin.VerifyCode))
            {
                msg.Code = 102;
                msg.Msg = "验证码不能为空";

                return msg;
            }

            if (adminLogin.VerifyCode.Length > 6)
            {
                msg.Code = 102;
                msg.Msg = "验证码输入错误";

                return msg;
            }

            var validate = HttpSession.EqualsSessionValue(Consts.Session_ValidateCode, adminLogin.VerifyCode);
            HttpSession.RemoveSession(Consts.Session_ValidateCode);
            if(!validate)
            {
                msg.Code = 103;
                msg.Msg = "验证码错误";

                return msg;
            }

            var admin = this.GetAdminByUserName(adminLogin.UserName);
            if (admin == null || admin.ID <= 0)
            {
                msg.Code = 11;
                msg.Msg = "用户名或密码错误";

                return msg;
            }

            if (admin.State == 2)
            {
                msg.Code = 12;
                msg.Msg = "用户已禁用";

                return msg;
            }

            var timeStamp = (int)DateTimeUtil.DateTimeToUnixTimeStamp(DateTime.Now);
            if (admin.LockTime > timeStamp)
            {
                msg.Code = 13;
                msg.Msg = $"帐号已锁定，请{LogonSettings.Value.LockMinute}分钟后再来登录";

                return msg;
            }

            adminLogin.Password = EncryptMd5.EncryptByte(adminLogin.Password);
            if (!admin.Password.Equals(adminLogin.Password))
            {
                if (admin.ErrorLogonTime + (LogonSettings.Value.ErrorTime * 60) < timeStamp)
                {
                    admin.ErrorLogonTime = timeStamp;
                    admin.ErrorLogonCount = 1;
                }
                else
                {
                    admin.ErrorLogonCount += 1;
                }

                if (admin.ErrorLogonCount >= LogonSettings.Value.ErrorCount)
                {
                    admin.ErrorLogonTime = 0;
                    admin.ErrorLogonCount = 0;
                    admin.LockTime = timeStamp + (LogonSettings.Value.LockMinute * 60);

                    //锁定帐号
                    this.LockAdmin(admin.ID, admin.LockTime);

                    msg.Code = 14;
                    msg.Msg = $"帐号或密码在{LogonSettings.Value.ErrorTime}分钟内，错误{LogonSettings.Value.ErrorCount}次，锁定帐号{LogonSettings.Value.LockMinute}分钟";

                    return msg;
                }
                else
                {
                    //更新错误登录信息
                    this.UpdateErrorLogon(admin.ID, admin.ErrorLogonTime, admin.ErrorLogonCount);

                    msg.Code = 15;
                    msg.Msg = $"帐号或密码错误，如在{LogonSettings.Value.ErrorTime}分钟内，错误{LogonSettings.Value.ErrorCount}次，将锁定帐号{LogonSettings.Value.LockMinute}分钟";

                    return msg;
                }
            }

            admin.LastLogonTime = timeStamp;
            admin.ErrorLogonTime = 0;
            admin.ErrorLogonCount = 0;
            admin.LockTime = 0;

            //更新管理员登录信息
            this.UpdateAdminLogon(admin.ID, admin.LastLogonTime);

            CMSAdminCookie.SetAdiminCookie(adminLogin);

            msg.Code = 0;
            msg.Msg = "登录成功";
            return msg;
        }

        //登出
        public void AdminLogout()
        {
            CMSAdminCookie.DelAdiminCookie();
        }

        //是否登录（true 登录在线，false 离线）
        public bool VerifyAdminLogin()
        {
            var adminToken = CMSAdminCookie.GetAdiminCookie();

            if (adminToken == null || string.IsNullOrEmpty(adminToken.UserName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
