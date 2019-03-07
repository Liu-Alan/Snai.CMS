using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Utils
{
    public class RandomUtils
    {
        //随机数串
        private static string randomChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        //生成随机数
        public static string CreateRandom(int length)
        {
            string rndCode = "";
            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                rndCode += randomChars.Substring(rnd.Next(0, randomChars.Length), 1);
            }

            return rndCode;
        }
    }
}
