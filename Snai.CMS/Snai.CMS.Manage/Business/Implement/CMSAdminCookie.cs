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
using Snai.CMS.Manage.Common.Infrastructure.HttpContexts;
using Snai.CMS.Manage.Common;

namespace Snai.CMS.Manage.Business.Implement
{
    public class CMSAdminCookie : ICMSAdminCookie
    {
        #region 属性声明

        IOptions<WebSettings> WebSettings;
        public IHttpCookie HttpCookie;

        #endregion

        #region 构造函数

        public CMSAdminCookie(IOptions<WebSettings> webSettings, IHttpCookie httpCookie)
        {
            WebSettings = webSettings;
            HttpCookie = httpCookie;
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
                RandomCode = RandomUtil.CreateRandom(16)
            };

            string tokrn = JsonConvert.SerializeObject(adminToken);

            string cipherTokrn = EncryptAES.Encrypt(WebSettings.Value.CipherKey,tokrn);

            HttpCookie.SetCookie(Consts.Cookie_AdminToken, cipherTokrn);
        }

        //读取Cookie
        public AdminToken GetAdiminCookie()
        {
            var adminToken = HttpCookie.GetCookie<AdminToken>(Consts.Cookie_AdminToken);

            return adminToken;
        }

        //删除Cookie
        public void DelAdiminCookie()
        {
            HttpCookie.DelCookie(Consts.Cookie_AdminToken);
        }

        #endregion
    }
}
