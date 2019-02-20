using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Snai.CMS.Manage.Common.Infrastructure.Filters;
using Snai.CMS.Manage.Models;

namespace Snai.CMS.Manage.Controllers
{
    public class HomeController : Controller
    {
        [ServiceFilter(typeof(AuthorizationFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
