using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Utils
{
    public class DateTimeUtil
    {
        /// <summary>
        /// 将时间转换成unix时间戳
        /// </summary>
        /// <param name="time">本地时间</param>
        /// <returns>返回单位秒</returns>
        public static long DateTimeToUnixTimeStamp(DateTime time)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return Convert.ToInt64((time - startTime).TotalSeconds);
        }

        /// <summary>
        /// 将unix时间戳转换成时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long timeStamp)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return startTime.AddSeconds(timeStamp);
        }
    }
}
