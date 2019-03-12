using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snai.CMS.Manage.Common.Utils;
using Snai.CMS.Manage.Common.Encrypt;
using Snai.CMS.Manage.Business.Interface;
using Snai.CMS.Manage.Entities.Settings;
using Microsoft.Extensions.Options;
using Snai.CMS.Manage.Common;
using Snai.CMS.Manage.Common.Infrastructure.Extension;

namespace Snai.CMS.Manage.Business.Implement
{
    public class CMSAdminCookie : ICMSAdminCookie
    {
        #region 属性声明

        IOptions<WebSettings> WebSettings;
        IHttpContextExtension HttpExtension;

        #endregion

        #region 构造函数

        public CMSAdminCookie(IOptions<WebSettings> webSettings, IHttpContextExtension httpExtension)
        {
            WebSettings = webSettings;
            HttpExtension = httpExtension;
        }

        #endregion

        #region AdminCookie操作

        //设置Cookie
        public void SetAdiminCookie(AdminLogin admin)
        {
            AdminToken adminToken = new AdminToken
            {
                UserName = admin.UserName,
                Password = admin.Password,
                RandomCode = RandomUtils.CreateRandom(16)
            };

            string tokrn = JsonConvert.SerializeObject(adminToken);

            string cipherToken = EncryptAES.Encrypt(WebSettings.Value.CipherKey,tokrn);

            HttpExtension.SetCookie(Consts.Cookie_AdminToken, cipherToken);
        }

        //读取Cookie
        public AdminToken GetAdiminCookie()
        {
            var token = HttpExtension.GetCookie(Consts.Cookie_AdminToken);

            var plainerToken = EncryptAES.Decrypt(WebSettings.Value.CipherKey, token);

            var adminToken = JsonConvert.DeserializeObject<AdminToken>(plainerToken);

            return adminToken;
        }

        //删除Cookie
        public void DelAdiminCookie()
        {
            HttpExtension.DelCookie(Consts.Cookie_AdminToken);
        }

        #endregion
    }
}
