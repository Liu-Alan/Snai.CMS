using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snai.CMS.Manage.Entities.BackManage
{
    [Table("modules")]
    public class Module
    {
        public Module()
        {
            ID = 0;
            ParentID = 0;
            ParentTitle = "";
            Title = "";
            Controller = "";
            Action = "";
            Sort = 1;
            State = 1;
        }

        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("parent_id")]
        public int ParentID { get; set; }

        [NotMapped]
        public string ParentTitle { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("controller")]
        public string Controller { get; set; }

        [Column("action")]
        public string Action { get; set; }

        [Column("sort")]
        public int Sort { get; set; }

        [Column("state")]
        public byte State { get; set; }
    }
}
