using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("category")]
    public partial class Category
    {
        [Key]
        [Column("cat_id")]
        [Required]
        public int CatId { get; set; }
        [Column("cat_add_date")]
        public DateTime? CatAddDate { get; set; }
        [Column("cat_deleted")]
        [DefaultValue("false")]
        public bool? CatDeleted { get; set; }
        [Column("cat_name")]
        [StringLength(100)]
        [Required]
        public string? CatName { get; set; }
        [Column("cat_description")]
        [StringLength(1000)]        
        public string? CatDescription { get; set; }
        public virtual ICollection<BlogEntry>? BlogEntries { get; set; } = new HashSet<BlogEntry>();
    }
}
