using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Entities.Settings
{
    public class LogonSettings
    {
        //错误次数
        public int ErrorCount { get; set; }

        //锁定时间（分钟）
        public int LockMinute { get; set; }
    }
}
