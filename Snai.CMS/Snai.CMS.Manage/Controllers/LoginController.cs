using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Snai.CMS.Manage.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult AdminLogin()
        {
            return View();
        }
    }
}