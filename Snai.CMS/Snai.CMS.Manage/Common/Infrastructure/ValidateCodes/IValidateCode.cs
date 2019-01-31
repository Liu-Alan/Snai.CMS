using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Common.Infrastructure.ValidateCodes
{
    public interface IValidateCode
    {
        byte[] CreateImage(out string code, int length = 4);
    }
}
