using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("user")]
    public partial class User
    {
        [Key]
        public int UsrId { get; set; }
        [Column("usr_add_date")]
        public DateTime? UsrAddDate { get; set; }
        [Column("usr_deleted")]
        [DefaultValue("false")]
        public bool? UsrDeleted { get; set; }
        [Column("usr_first_name")]
        [StringLength(50)]
        [Required]
        public string UsrFirstName  { get; set; }
        [Column("usr_last_name")]
        [StringLength(50)]
        public string? UsrLastName { get; set; }
        [Column("usr_email")]
        [StringLength(100)]
        [Required]
        public string UsrEmail { get; set; }
        [Column("usr_password")]
        [StringLength(250)]
        [Required]
        public string UsrPassword { get; set; }
        public virtual ICollection<BlogEntry>? BlogEntries { get; set; } = new HashSet<BlogEntry>();
    }
}
