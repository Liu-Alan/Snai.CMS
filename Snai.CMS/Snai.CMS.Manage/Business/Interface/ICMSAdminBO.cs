using Snai.CMS.Manage.Common.Infrastructure;
using Snai.CMS.Manage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Business.Interface
{
    public interface ICMSAdminBO
    {
        //添加管理员
        Message CreateAdmin(Admin admin);
    }
}
