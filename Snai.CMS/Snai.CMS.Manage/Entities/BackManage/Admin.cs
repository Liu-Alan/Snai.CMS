using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Entities.BackManage
{
    [Table("admins")]
    public class Admin
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("user_name")]
        public string UserName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [NotMapped]
        public string RePassword { get; set; }

        [Column("state")]
        public byte State { get; set; }

        [Column("role_id")]
        public int RoleID { get; set; }

        [Column("create_time")]
        public int CreateTime { get; set; }

        [Column("update_time")]
        public int UpdateTime { get; set; }

        [Column("last_logon_time")]
        public int LastLogonTime { get; set; }

        [Column("error_logon_time")]
        public int ErrorLogonTime { get; set; }

        [Column("error_logon_count")]
        public int ErrorLogonCount { get; set; }

        [Column("lock_time")]
        public int LockTime { get; set; }
    }
}
