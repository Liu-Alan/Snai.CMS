using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Utils
{
    public class TextUtils
    {
        #region 字符串比较

        /// <summary>
        /// 判断是否是同一个的字符组成的字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lookfor"></param>
        /// <returns></returns>
        public static bool EqualsSameChar(string text, char lookfor)
        {
            if (text.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != lookfor)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
