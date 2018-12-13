using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Snai.CMS.Manage.Common.Utils
{
    public sealed class Validator
    {
        #region 正则变量

        private static readonly Regex regex_IsLetters = new Regex(@"^[a-zA-Z]+$");
        private static readonly Regex regex_IsContainChars = new Regex(@"[A-Za-z]+");
        private static readonly Regex regex_IsNumbers = new Regex(@"^[0-9]+$");

        #endregion

        #region 判断是否是字符数字及汉字

        /// <summary>
        /// 是否大小写字母组合
        /// </summary>
        /// <param name="expression">要确认的字符串</param>
        /// <returns>是则返回true 否则返回false</returns>
        public static bool IsLetters(string expression)
        {
            return regex_IsLetters.IsMatch(expression);
        }

        /// <summary>
        /// 是否数字组合
        /// </summary>
        /// <param name="expression">要确认的字符串</param>
        /// <returns>是则返回true 否则返回false</returns>
        public static bool IsNumbers(string expression)
        {
            return regex_IsNumbers.IsMatch(expression);
        }

        /// <summary>
        /// 是否包含字母(a-z或A-Z)
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static bool IsContainChars(string strVal)
        {
            return regex_IsContainChars.IsMatch(strVal);
        }

        #endregion

        #region 判断密码

        /// <summary>
        /// 是否是密码 至少6位，必须是字母(a-z或A-Z)与(数字(0-9)或特殊符号)组合
        ///
        /// 0	密码正确
        /// 1	包含空格 全角或半角
        /// 2	字符长度不足
        /// 4	密码为空
        /// 5	单个字符重复出现
        /// 6	递增或递减的数字或字母
        /// 7	密码属于社会工程学中
        /// 8   密码包含字母但不能是纯字母
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static int IsPassword(string strVal)
        {
            int minL = 6;
            int maxL = 100;

            //密码为空
            if (string.IsNullOrEmpty(strVal))
            {
                return 4;
            }

            //长度不足
            if (strVal.Length < minL || strVal.Length > maxL)
            {
                return 2;
            }

            //包含字母但不能是纯字母
            if (!Validator.IsContainChars(strVal))
            {
                return 8;
            }
            if (Validator.IsLetters(strVal))
            {
                return 8;
            }

            //是否包含空格 或 其他全角字符
            int index;
            int charCount = 0;
            int charId;
            for (index = 0; index < strVal.Length; index++)
            {
                charId = char.ConvertToUtf32(strVal, index);
                if (charId <= 32 || charId > 126)
                {
                    charCount = 1;
                    break;
                }
            }
            if (charCount == 1)
            {
                return 1;
            }

            //是否单个字符重复出现
            if (TextUtil.EqualsSameChar(strVal, strVal[0]))
            {
                return 5;
            }

            //是否递增或递减的数字或字母
            int firstCharId = char.ConvertToUtf32(strVal, 0);
            index = 0;
            charCount = 0;
            if (Validator.IsNumbers(strVal) || Validator.IsLetters(strVal))
            {
                charId = char.ConvertToUtf32(strVal, strVal.Length - 1);

                //递减
                int vector = -1;
                if (firstCharId < charId)
                {
                    //递增
                    vector = 1;
                }

                while (index < strVal.Length)
                {
                    if (char.ConvertToUtf32(strVal, index) + vector == char.ConvertToUtf32(strVal, index + 1))
                    {
                        charCount++;
                    }
                    else
                    {
                        break;
                    }

                    index++;
                }

                if ((charCount + 1) == strVal.Length)
                {
                    return 6;
                }
            }

            //是否是社会工程字典中的密码
            index = 0;
            charCount = 0;
            string[] lvDicts = new string[] { "asdfg", "asdfgh", "qwert", "qwerty", "zxcvb", "zxcvbn", "asdf", "qwer", "zxcv", "password", "passwd", "test", "woaini", "iloveyou", "woaiwojia", "521521", "5201314", "7758521", "1314520", "1314521", "520520", "201314", "211314", "7758258", "147258369", "159357", "12345", "123456", "1234567", "12345678", "123456789", "654321", "123123", "123321", "123abc" };

            for (index = 0; index < lvDicts.Length; index++)
            {
                if (strVal == lvDicts[index])
                {
                    return 7;
                }
            }

            return 0;
        }

        #endregion 判断密码
    }
}
