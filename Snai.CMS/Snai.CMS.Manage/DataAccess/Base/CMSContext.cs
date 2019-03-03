using Microsoft.EntityFrameworkCore;
using Snai.CMS.Manage.Entities.BackManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.DataAccess.Base
{
    public class CMSContext: DbContext
    {
        public CMSContext(DbContextOptions<CMSContext> options)
            : base(options)
        { }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Module> Modules { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<RoleRight> RoleRights{ get; set; }
    }
}
